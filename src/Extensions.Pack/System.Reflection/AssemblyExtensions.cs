using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Argument.Check;

namespace Extensions.Pack
{
    public class EmbededResuorceNotFoundException : Exception
    {
        internal EmbededResuorceNotFoundException(string message) : base(message)
        {
        }
    }

    public class InvalidJsonException : Exception
    {
        internal InvalidJsonException(string message) : base(message)
        {
        }
    }

    public record InMemoryFileAsByteArray(byte[] FileContent,
                                          string Name);

    public record InMemoryFileAsString(string FileContent,
                                       string Name);

    public record EmbeddedFileStream(MemoryStream Stream,
                                     string Name) : IDisposable
    {
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Stream.Dispose();
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public record InMemoryFileAsStream(MemoryStream FileStream,
                                       string FileName) : IAsyncDisposable
    {
        public async ValueTask DisposeAsync()
        {
            await FileStream.DisposeAsync().ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }
    }

    public static class EmbeddedFile
    {
        public static string GetFileContentFrom(string fileName)
        {
            return GetFileContentFrom(Assembly.GetCallingAssembly(), fileName);
        }

        public static string GetFileContentFrom(this Assembly assembly,
                                                string fileName)
        {
            return assembly.GetJsonFileContentFrom(fileName);
        }

        public static string GetFileContentOrDefaultFrom(string fileName,
                                                         string defaultValue = "")
        {
            return GetFileContentOrDefaultFrom(Assembly.GetCallingAssembly(), fileName, defaultValue);
        }

        public static string GetFileContentOrDefaultFrom(this Assembly assembly,
                                                         string fileName,
                                                         string defaultValue = "")
        {
            return assembly.GetJsonFileContentOrDefaultFrom(fileName, defaultValue);
        }
    }

    public static class AssemblyExtensions
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public static T? ReadAs<T>(this object assembly,
                                   string fileName) where T : class
        {
            Throw.IfNull(assembly);
            Throw.IfNullOrWhiteSpace(fileName);

            return assembly.GetType().Assembly.ReadAs<T>(fileName);
        }

        public static T? ReadAs<T>(this Assembly assembly,
                                   string fileName) where T : class
        {
            Throw.IfNull(assembly);
            Throw.IfNullOrWhiteSpace(fileName);

            var result = assembly.GetFileAsByteArrayFrom(fileName);

            using var memoryStream = new MemoryStream(result.FileContent);
            using var streamReader = new StreamReader(memoryStream);
            var stringContent = streamReader.ReadToEnd();

            return JsonSerializer.Deserialize<T>(stringContent, JsonSerializerOptions);
        }

        public static string GetJsonFileContentFrom(this Assembly assembly,
                                                    string fileName)
        {
            Throw.IfNull(assembly);
            Throw.IfNullOrWhiteSpace(fileName);

            var result = assembly.GetFileAsByteArrayFrom(fileName);
            using var memoryStream = new MemoryStream(result.FileContent);
            using var streamReader = new StreamReader(memoryStream);

            return streamReader.ReadToEnd();
        }

        public static string GetJsonFileContentOrDefaultFrom(this Assembly assembly,
                                                             string fileName,
                                                             string defaultValue = "")
        {
            Throw.IfNull(assembly);
            Throw.IfNullOrWhiteSpace(fileName);

            var result = assembly.GetFileAsByteArrayOrDefaultFrom(fileName);

            if (result.IsNull())
            {
                return defaultValue;
            }

            using var memoryStream = new MemoryStream(result.FileContent);
            using var streamReader = new StreamReader(memoryStream);

            return streamReader.ReadToEnd();
        }

        // very important try to avoid accessing file system during tests, so we fetch our data from
        // our assembly direct from the memory :-) 
        private static InMemoryFileAsByteArray GetFileAsByteArrayFrom(this Assembly assembly,
                                                                      string fileName)
        {
            var manifestResourceNames = assembly.GetManifestResourceNames();
            var name = manifestResourceNames.FirstOrDefault(name => name.ToLowerInvariant().Contains($"{fileName.ToLowerInvariant()}", StringComparison.InvariantCulture));

            if (name.IsNull())
            {
                throw new EmbededResuorceNotFoundException($"Embedded resource with name: '{fileName}' does not exists. Available for your assembly: '{assembly.GetName().Name}' are: {Environment.NewLine}{manifestResourceNames.Flatten(Environment.NewLine)}");
            }

            // steam can not be null check before validates that embedded resource exists.
            using var stream = assembly.GetManifestResourceStream(name);
            using var ms = new MemoryStream();
            stream!.CopyTo(ms);

            return new InMemoryFileAsByteArray(ms.ToArray(), name);
        }

        private static InMemoryFileAsByteArray? GetFileAsByteArrayOrDefaultFrom(this Assembly assembly,
                                                                                string fileName)
        {
            var manifestResourceNames = assembly.GetManifestResourceNames();
            var name = manifestResourceNames.FirstOrDefault(name => name.ToLowerInvariant().Contains($"{fileName.ToLowerInvariant()}", StringComparison.InvariantCulture));

            if (name.IsNull())
            {
                return null;
            }

            // steam can not be null check before validates that embedded resource exists.
            using var stream = assembly.GetManifestResourceStream(name);
            using var ms = new MemoryStream();
            stream!.CopyTo(ms);

            return new InMemoryFileAsByteArray(ms.ToArray(), name);
        }

        public static bool IsCompiledInDebug(this Assembly assembly)
        {
            return assembly.GetCustomAttribute<DebuggableAttribute>()?.IsJITTrackingEnabled ?? false;
        }

        public static async Task<InMemoryFileAsString> GetEmbeddedFileAsStringAsync(this Assembly assembly,
                                                                                    string embededResourceName)
        {
            var result = await assembly.GetFileAsByteArrayFromAsync(embededResourceName).ConfigureAwait(false);
            using var memoryStream = new MemoryStream(result);
            using var streamReader = new StreamReader(memoryStream);
            var stringContent = await streamReader.ReadToEndAsync().ConfigureAwait(false);

            return new InMemoryFileAsString(stringContent, embededResourceName);
        }

        public static async Task<InMemoryFileAsByteArray> GetEmbeddedFileAsByteArrayAsync(this Assembly assembly,
                                                                                          string embededResourceName)
        {
            var bytes = await assembly.GetFileAsByteArrayFromAsync(embededResourceName).ConfigureAwait(false);

            return new InMemoryFileAsByteArray(bytes, embededResourceName);
        }

        public static EmbeddedFileStream GetEmbeddedFileStream(this Assembly assembly,
                                                               string fileName)
        {
            var stream = assembly.GetEmbeddedFileAsStream(fileName);

            return new EmbeddedFileStream(stream, fileName);
        }

        public static MemoryStream GetEmbeddedFileAsStream(this Assembly assembly,
                                                           string fileName)
        {
            Throw.IfNull(assembly);
            Throw.IfNullOrWhiteSpace(fileName);

            var manifestResourceNames = assembly.GetManifestResourceNames();
            var name = manifestResourceNames.FirstOrDefault(name => name.Contains(fileName));

            if (name.IsNull())
            {
                throw new InvalidOperationException($"Unable to locate the file: '{fileName}'. The '{assembly.GetName()}', does not contains the requested embedded resource. Available are: '{manifestResourceNames.Flatten(";")}'");
            }

            using var manifestResourceStream = assembly.GetManifestResourceStream(name);
            Throw.IfNull(manifestResourceStream);

            var memoryStream = new MemoryStream();
            manifestResourceStream.CopyTo(memoryStream);
            memoryStream.Position = 0;

            return memoryStream;
        }

        public static string GetJsonString<T>(this string jsonValueOrEmbeddedFile,
                                              Assembly callingAssembly)
        {
            // 1. Get target type
            var targetType = typeof(T);

            // 2. Check if target type is an enumerable
            var isEnumerable = targetType.IsEnumerable();

            // 3. If the given json value is null or empty then return it
            //    Default serialization will be handled by the caller
            if (jsonValueOrEmbeddedFile.IsNullOrWhiteSpace())
            {
                return jsonValueOrEmbeddedFile;
            }

            // 4. Trim the strings
            var trimmedJsonValue = jsonValueOrEmbeddedFile.Trim() // Trim whitespaces
                                                          .TrimEnd(Environment.NewLine.ToCharArray()) // Trim line breaks at the end if exists
                                                          .Trim('"'); // Trim " if exists cause not needed

            // 5. If it is a json file then read the content of the file
            if (trimmedJsonValue.EndWith(".json"))
            {
                trimmedJsonValue = callingAssembly.GetFileContentFrom(trimmedJsonValue).Trim().TrimEnd(Environment.NewLine.ToCharArray());
            }

            // 6. If the json string is an array but the target type is not an enumerable then throw an exception
            if (trimmedJsonValue.StartWith("[") &&
                trimmedJsonValue.EndWith("]") &&
                isEnumerable.IsFalse())
            {
                throw new InvalidJsonException($"Your passed json string: {trimmedJsonValue} is an array notation [], but your target type: {targetType} is not an array so you can't deserialize it. Please fix your json string");
            }

            // 7. If the json string is an object but the target type is an enumerable then throw an exception
            if (trimmedJsonValue.StartWith("{") &&
                trimmedJsonValue.EndWith("}") &&
                isEnumerable)
            {
                throw new InvalidJsonException($"Your passed json string: {trimmedJsonValue} is an object notation {{}}, but your target type: {targetType} is an array so you can't deserialize it. Please fix your json string");
            }

            // 8. If json notation is fine so return it.
            if ((trimmedJsonValue.StartWith("{") && trimmedJsonValue.EndWith("}")) ||
                (trimmedJsonValue.StartWith("[") && trimmedJsonValue.EndWith("]")))
            {
                return trimmedJsonValue;
            }

            // 9. If the target type is a primitive type or a string then return the json value
            var type = typeof(T);

            if (type.IsPrimitive || type.EqualsTo(typeof(string)))
            {
                return jsonValueOrEmbeddedFile;
            }

            throw new InvalidJsonException($"Your given json string does not contains a valid json string. Json strings have to begin with '{{' and end with a '}}' or if you use an array notation then []{Environment.NewLine}Your invalid string is:{Environment.NewLine}{trimmedJsonValue}");
        }

        private static async Task<byte[]> GetFileAsByteArrayFromAsync(this Assembly assembly,
                                                                      string fileName)
        {
            var stream = assembly.GetEmbeddedFileAsStream(fileName);

            await using (stream.ConfigureAwait(false))
            {
                var ms = new MemoryStream();

                await using (ms.ConfigureAwait(false))
                {
                    await stream.CopyToAsync(ms).ConfigureAwait(false);

                    return ms.ToArray();
                }
            }
        }
    }
}
