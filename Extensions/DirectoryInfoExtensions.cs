using System.IO;

namespace Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static void Clean(this DirectoryInfo directory)
        {
            directory.GetFiles().Each(file => file.Delete());
            directory.GetDirectories().Each(subDirectory => subDirectory.Delete(true));
        }
    }
}
