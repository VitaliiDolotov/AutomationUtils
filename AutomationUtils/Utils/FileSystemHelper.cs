using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace AutomationUtils.Utils
{
    public class FileSystemHelper
    {
        public static void EnsureFolderExists(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public static string GetRecentFilesWithNameContains(string dirPath, string partOfFileName)
        {
            for (var i = 0; i < 15; i++)
            {
                try
                {
                    return GetFilesWithNamePart(dirPath, partOfFileName)?.Select(x => x.FullName).First();
                }
                catch
                {
                    // ignored
                }
                Thread.Sleep(1000);
            }

            throw new Exception($"File with '{partOfFileName}' name part was not downloaded");
        }

        public static List<FileInfo> GetFilesWithNamePart(string dirPath, string partOfFileName)
        {
            var directory = new DirectoryInfo(dirPath);
            var files = directory.GetFiles($"*{partOfFileName}*.*");
            if (!files.Any())
            {
                throw new Exception($"There are not files with '{partOfFileName}' name part");
            }

            var filesInfo = files.OrderByDescending(f => f.LastWriteTime).ToList();
            return filesInfo;
        }
    }
}
