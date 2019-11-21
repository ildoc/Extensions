using System.IO;
using System.IO.Compression;

namespace Utils
{
    public static class Zip
    {
        public static void ZipDirectory(string dest, string source) => ZipFile.CreateFromDirectory(source, dest);

        public static void ZipFiles(string dest, params string[] files)
        {
            using (var za = ZipFile.Open(dest, ZipArchiveMode.Create))
                foreach (var file in files)
                    za.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Fastest);
        }

        public static void Unzip(string zipfilePath, string destination)
        {
            Directory.CreateDirectory(destination);
            var file = File.ReadAllBytes(zipfilePath);

            using (var archive = new ZipArchive(new MemoryStream(file), ZipArchiveMode.Read, true))
                archive.ExtractToDirectory(destination);
        }
    }
}
