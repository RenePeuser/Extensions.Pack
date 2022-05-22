using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Extensions.Pack
{
    public record InMemoryFileAsByte(byte[] FileContent, string Name);

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
        public static async Task<string> GetEmbeddedFileAsStringAsync(this Assembly assembly, string embededResourceName)
        {
            var result = await assembly.GetFileAsByteArrayFromAsync(embededResourceName).ConfigureAwait(false);
            using var streamReader = new StreamReader(new MemoryStream(result));
            return await streamReader.ReadToEndAsync().ConfigureAwait(false);
        }

        public static Task<byte[]> GetEmbeddedFileAsByteArrayAsync(this Assembly assembly, string embededResourceName)
        {
            return assembly.GetFileAsByteArrayFromAsync(embededResourceName);
        }

        public static async Task<InMemoryFileAsByte> GetEmbeddedFileAsByteAsync(this Assembly assembly, string embededResourceName)
        {
            var bytes = await assembly.GetFileAsByteArrayFromAsync(embededResourceName).ConfigureAwait(false);
            return new InMemoryFileAsByte(bytes, embededResourceName);
        }

        public static EmbeddedFileStream GetEmbeddedFileStream(this Assembly assembly, string fileName)
        {
            var stream = assembly.GetEmbeddedFileAsStream(fileName);
            return new EmbeddedFileStream(stream, fileName);
        }

        public static Stream GetEmbeddedFileAsStream(this Assembly assembly, string fileName)
        {
            var manifestResourceNames = assembly.GetManifestResourceNames();
            var name = manifestResourceNames.FirstOrDefault(name => name.Contains(fileName));
            if (name.IsNull())
            {
                throw new InvalidOperationException($"Unable to locate the file: '{fileName}'. The '{assembly.GetName()}', does not contains the requested embedded resource. Available are: '{manifestResourceNames.Flatten(";")}'");
            }

            return assembly.GetManifestResourceStream(name);
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
