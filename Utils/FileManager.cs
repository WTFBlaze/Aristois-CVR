using System.IO;
using System.Text;

namespace Aristois.Utils
{
    public static class FileManager
    {
        public static void CreateFile(string location)
            => File.Create(location).Close();

        public static void DeleteFile(string location)
            => File.Delete(location);

        public static void AppendLineToFile(string location, string text)
        {
            using FileStream file = new(location, FileMode.Append, FileAccess.Write, FileShare.Read);
            using StreamWriter writer = new(file, Encoding.Unicode);
            writer.Write(text + "\n");
        }

        public static void AppendTextToFile(string location, string text)
        {
            using FileStream file = new(location, FileMode.Append, FileAccess.Write, FileShare.Read);
            using StreamWriter writer = new(file, Encoding.Unicode);
            writer.Write(text);
        }

        public static void WriteAllToFile(string location, string text)
        {
            using FileStream file = new(location, FileMode.Open, FileAccess.Write, FileShare.Read);
            using StreamWriter writer = new(file, Encoding.Unicode);
            writer.Write(text);
        }

        public static void WriteAllBytesToFile(string location, byte[] bytes)
            => File.WriteAllBytes(location, bytes);

        public static void WipeTextFromFile(string location)
            => File.WriteAllText(location, string.Empty);

        public static void CopyFile(string originalFile, string newFile)
            => File.Copy(originalFile, newFile);

        public static void RenameFile(string file, string newName)
            => File.Move(file, newName);

        public static string ReadAllOfFile(string location)
            => File.ReadAllText(location);

        public static string[] ReadAllLinesOfFile(string location)
            => File.ReadAllLines(location);

        public static byte[] ReadAllBytesOfFile(string location)
            => File.ReadAllBytes(location);
    }
}
