using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Extensions.Pack
{
    public static class AssemblyExtensions
    {
        [Obsolete("Use new method 'Task<string> GetEmbeddedFileAsStringAsync(this Assembly assembly, string embededResourceName)' instead.")]
        public static string GetResourceAsString(this Assembly assembly, string resourceName)
        {
            Throw.IfNull(() => assembly);
            Throw.IfNullOrEmpty(() => resourceName);

            var resource = assembly.GetManifestResourceNames().FirstOrDefault(n => n.EqualsTo(resourceName));
            Throw.If(() => resource, r => r.IsNull(), $"The resource: '{resourceName}' was not found in the assembly: '{assembly.GetName()}'");

            using var stream = assembly.GetManifestResourceStream(resource);
            using var streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd();
        }

        public static async Task<string> GetEmbeddedFileAsStringAsync(this Assembly assembly, string embededResourceName)
        {
            var result = await assembly.GetFileAsByteArrayFromAsync(embededResourceName).ConfigureAwait(false);
            using var streamReader = new StreamReader(new MemoryStream(result));
            return await streamReader.ReadToEndAsync().ConfigureAwait(false);
        }

        private static async Task<byte[]> GetFileAsByteArrayFromAsync(this Assembly assembly, string fileName)
        {
            var name = assembly.GetManifestResourceNames().First(name => name.Contains(fileName));
            await using var stream = assembly.GetManifestResourceStream(name);
            await using var ms = new MemoryStream();
            await stream!.CopyToAsync(ms).ConfigureAwait(false);
            return ms.ToArray();
        }
    }
}
