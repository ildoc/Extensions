using System;
using System.IO;

namespace Utils
{
    public static class _
    {
        public static bool Try(Action a)
        {
            try
            {
                a();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static T Try<T>(Func<T> f)
        {
            try
            {
                return f();
            }
            catch
            {
            }
            return default;
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs = false, bool overWrite = false)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            var dirs = dir.GetDirectories();
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            foreach (var file in dir.GetFiles())
            {
                var temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, overWrite);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (var subdir in dirs)
                {
                    var temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
