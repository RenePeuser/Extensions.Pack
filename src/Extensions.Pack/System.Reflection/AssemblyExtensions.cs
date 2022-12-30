using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
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

    public record InMemoryFileAsByteArray(byte[] FileContent, string Name);

    public record InMemoryFileAsString(string FileContent, string Name);

    public record EmbeddedFileStream(MemoryStream Stream, string Name) : IDisposable
    {
        public void Dispose()
        {
            Stream.Dispose();
            GC.SuppressFinalize(this);
        }
    }


    public record InMemoryFileAsStream(MemoryStream FileStream, string FileName) : IAsyncDisposable
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

        public static string GetFileContentFrom(this Assembly assembly, string fileName)
        {
            return assembly.GetJsonFileContentFrom(fileName);
        }
    }

    public static class AssemblyExtensions
    {
        public static T? ReadAs<T>(this object assembly, string fileName) where T : class
        {
            return assembly.GetType().Assembly.ReadAs<T>(fileName);
        }

        public static T? ReadAs<T>(this Assembly assembly, string fileName) where T : class
        {
            var result = assembly.GetFileAsByteArrayFrom(fileName);
            using var streamReader = new StreamReader(new MemoryStream(result.FileContent));
            var stringContent = streamReader.ReadToEnd();
            return JsonSerializer.Deserialize<T>(stringContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public static string GetJsonFileContentFrom(this Assembly assembly, string fileName)
        {
            var result = assembly.GetFileAsByteArrayFrom(fileName);
            using var streamReader = new StreamReader(new MemoryStream(result.FileContent));
            return streamReader.ReadToEnd();
        }

        // very important try to avoid accessing file system during tests, so we fetch our data from
        // our assembly direct from the memory :-) 
        private static InMemoryFileAsByteArray GetFileAsByteArrayFrom(this Assembly assembly, string fileName)
        {
            var manifestResourceNames = assembly.GetManifestResourceNames();
            var name = manifestResourceNames.FirstOrDefault(name => name.ToLower(CultureInfo.InvariantCulture).Contains($"{fileName.ToLower(CultureInfo.InvariantCulture)}"));
            if (name is null)
            {
                throw new EmbededResuorceNotFoundException($"Embeded resource with name: '{fileName}' does not exists. Available for your assembly: '{assembly.GetName().Name}' are: {Environment.NewLine}{manifestResourceNames.Flatten(Environment.NewLine)}");
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

        public static async Task<InMemoryFileAsString> GetEmbeddedFileAsStringAsync(this Assembly assembly, string embededResourceName)
        {
            var result = await assembly.GetFileAsByteArrayFromAsync(embededResourceName).ConfigureAwait(false);
            using var streamReader = new StreamReader(new MemoryStream(result));
            var stringContent = await streamReader.ReadToEndAsync().ConfigureAwait(false);
            return new InMemoryFileAsString(stringContent, embededResourceName);
        }

        public static async Task<InMemoryFileAsByteArray> GetEmbeddedFileAsByteArrayAsync(this Assembly assembly, string embededResourceName)
        {
            var bytes = await assembly.GetFileAsByteArrayFromAsync(embededResourceName).ConfigureAwait(false);
            return new InMemoryFileAsByteArray(bytes, embededResourceName);
        }

        public static EmbeddedFileStream GetEmbeddedFileStream(this Assembly assembly, string fileName)
        {
            var stream = assembly.GetEmbeddedFileAsStream(fileName);
            return new EmbeddedFileStream(stream, fileName);
        }

        public static MemoryStream GetEmbeddedFileAsStream(this Assembly assembly, string fileName)
        {
            Throw.IfNull(assembly);
            Throw.IfNullOrWhiteSpace(fileName);

            var manifestResourceNames = assembly.GetManifestResourceNames();
            var name = manifestResourceNames.FirstOrDefault(name => name.Contains(fileName));
            if (name.IsNull())
            {
                throw new InvalidOperationException($"Unable to locate the file: '{fileName}'. The '{assembly.GetName()}', does not contains the requested embedded resource. Available are: '{manifestResourceNames.Flatten(";")}'");
            }

            var manifestResourceStream = assembly.GetManifestResourceStream(name);
            Throw.IfNull(manifestResourceStream);

            return (MemoryStream)manifestResourceStream;
        }

        public static string GetJsonString(this string jsonValueOrEmbeddedFile, Assembly callingAssembly)
        {
            // ToDo: Current exception string.empty have to fixed soon
            // ToDo: Regex for start end check for  {} and []
            if (jsonValueOrEmbeddedFile.IsNullOrWhiteSpace())
            {
                return jsonValueOrEmbeddedFile;
            }

            var trimmedJsonValue = jsonValueOrEmbeddedFile.Trim().TrimEnd(Environment.NewLine.ToCharArray());

            if (trimmedJsonValue.EndWith(".json"))
            {
                var jsonValueFromEmbeddedFile = callingAssembly.GetFileContentFrom(trimmedJsonValue).Trim().TrimEnd(Environment.NewLine.ToCharArray());
                if ((jsonValueFromEmbeddedFile.StartWith("{") && jsonValueFromEmbeddedFile.EndWith("}")) || (jsonValueFromEmbeddedFile.StartWith("[") && jsonValueFromEmbeddedFile.EndWith("]")))
                {
                    return jsonValueFromEmbeddedFile;
                }

                throw new InvalidJsonException($"Your given embedded file: '{jsonValueOrEmbeddedFile}' does not contains a valid json string. Json strings have to begin with '{{' and end with a '}}' or if you use an array notation then []");
            }

            if ((trimmedJsonValue.StartWith("{") && trimmedJsonValue.EndWith("}")) || (trimmedJsonValue.StartWith("[") && trimmedJsonValue.EndWith("]")))
            {
                return trimmedJsonValue;
            }

            throw new InvalidJsonException($"Your given json string does not contains a valid json string. Json strings have to begin with '{{' and end with a '}}' or if you use an array notation then []{Environment.NewLine}Your invalid string is:{Environment.NewLine}{trimmedJsonValue}");
        }


        private static async Task<byte[]> GetFileAsByteArrayFromAsync(this Assembly assembly, string fileName)
        {
            var stream = assembly.GetEmbeddedFileAsStream(fileName);

            await using (stream.ConfigureAwait(false))
            {
                var ms = new MemoryStream();
                await using (ms.ConfigureAwait(false))
                {
                    await stream!.CopyToAsync(ms).ConfigureAwait(false);
                    return ms.ToArray();
                }
            }
        }
    }
}
