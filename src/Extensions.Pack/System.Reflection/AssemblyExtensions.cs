using System.IO;
using System.Linq;
using System.Reflection;

namespace Extensions.Pack
{
    public static class AssemblyExtensions
    {
        public static string GetResourceAsString(this Assembly assembly, string resourceName)
        {
            Throw.IfNull(() => assembly);
            Throw.IfNullOrEmpty(() => resourceName);

            var resource = assembly.GetManifestResourceNames().FirstOrDefault(n => n.EqualsTo(resourceName));
            Throw.If(() => resource, r => r.IsNull(), $"The resource: '{resourceName}' was not found in the assembly: '{assembly.GetName()}'");

            using (var stream = assembly.GetManifestResourceStream(resource))
            using (var streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
