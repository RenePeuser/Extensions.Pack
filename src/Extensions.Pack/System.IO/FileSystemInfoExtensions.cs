using Argument.Check;

namespace Extensions.Pack
{
    public static class FileSystemInfoExtensions
    {
        public static string NameWithoutExtension(this FileSystemInfo fileSystemInfo)
        {
            Throw.IfNull(fileSystemInfo);

            return fileSystemInfo.Name.Replace(fileSystemInfo.Extension, string.Empty);
        }

        public static bool NotExists(this FileSystemInfo fileSystemInfo)
        {
            Throw.IfNull(fileSystemInfo);

            return fileSystemInfo.Exists.Negate();
        }

        public static bool IsFile(this FileSystemInfo fileSystemInfo)
        {
            Throw.IfNull(fileSystemInfo);

            return fileSystemInfo.Is<FileInfo>();
        }

        public static bool IsDirectory(this FileSystemInfo fileSystemInfo)
        {
            Throw.IfNull(fileSystemInfo);

            return fileSystemInfo.Is<DirectoryInfo>();
        }
    }
}
