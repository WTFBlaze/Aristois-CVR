using System.IO;
using System;
using Aristois.Utils;
using System.Linq;
using System.Diagnostics;

namespace Aristois.Core
{
    internal static class Logs
    {
        public static bool IsSetup { get; private set; } = false;
        private static string LogFileName = string.Empty;

        public static void Initialize()
        {
            DirectoryInfo directoryInfo = new(PathManager.LogsDir);
            FileInfo oldestFile = null;
            if (directoryInfo.GetFiles().Count() == 5)
            {
                foreach (var file in directoryInfo.GetFiles())
                {
                    if (oldestFile == null || file.LastWriteTime < oldestFile.LastWriteTime)
                    {
                        oldestFile = file;
                    }
                }
            }
            if (oldestFile != null)
                FileManager.DeleteFile(oldestFile.FullName);
            var fileName = $"Logs-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.log";
            LogFileName = PathManager.LogsDir + $"\\{fileName}";
            FileManager.CreateFile(LogFileName);
            var latest = PathManager.ModDir + "\\Latest.log";
            if (File.Exists(latest))
                FileManager.DeleteFile(latest);
            FileManager.CreateFile(latest);
            IsSetup = true;
            Log($"{ColorManager.Console.Cyan}Aristois Logger Initialized! {ColorManager.Console.Yellow}Log File: {ColorManager.Console.DarkYellow}{fileName}");
        }

        public static void Log(string msg)
        {
            var dateTime = DateTime.Now.ToString("HH:mm:ss.fff");
            string result = $"{ColorManager.Console.Cyan}{dateTime} {ColorManager.Console.Gray}>> {ColorManager.Console.White}{msg}";
            Console.WriteLine(result);
            FileManager.AppendLineToFile(PathManager.ModDir + "\\Latest.log", RemoveColorCharacters(result));
            FileManager.AppendLineToFile(LogFileName, RemoveColorCharacters(result));
        }

        public static void Error(string msg)
            => Log($"{ColorManager.Console.Red}ERROR {ColorManager.Console.DarkRed} >>  {ColorManager.Console.Gray}{msg}");

        public static void Error(string msg, StackTrace st)
            => Log($"{ColorManager.Console.Red}ERROR {ColorManager.Console.DarkRed}>> {ColorManager.Console.Gray}{msg} | {st}");

        public static void Error(string msg, string stackTrace)
            => Log($"{ColorManager.Console.Red}ERROR {ColorManager.Console.DarkRed}>> {ColorManager.Console.Gray}{msg} | {stackTrace}");

        public static void Error(Exception ex)
            => Log($"{ColorManager.Console.Red}ERROR {ColorManager.Console.DarkRed}>> {ColorManager.Console.Gray}{ex.Message} | {ex.StackTrace}");

        private static string RemoveColorCharacters(string msg)
        {
            string result = msg;
            foreach (var field in typeof(ColorManager.Console).GetFields())
                result = result.Replace(field.GetValue(null).ToString(), string.Empty);
            return result;
        }
    }
}
