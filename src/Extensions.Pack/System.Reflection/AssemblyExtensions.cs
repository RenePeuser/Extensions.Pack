using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Argument.Check;

namespace Extensions.Pack
{
    public record InMemoryFileAsByteArray(byte[] FileContent, string Name);

    public record InMemoryFileAsString(string FileContent, string Name);

    public record EmbeddedFileStream(Stream Stream, string Name);


    public record InMemoryFileAsStream(Stream FileStream, string FileName) : IAsyncDisposable
    {
        public async ValueTask DisposeAsync()
        {
            await FileStream.DisposeAsync().ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }
    }

    public static class AssemblyExtensions
    {
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

        public static Stream GetEmbeddedFileAsStream(this Assembly assembly, string fileName)
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

            return manifestResourceStream;
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
