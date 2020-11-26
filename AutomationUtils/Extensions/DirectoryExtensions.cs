using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace AutomationUtils.Extensions
{
    public static class DirectoryExtensions
    {
        public static string GetFileWithNamePart(this DirectoryInfo directory, string partOfFileName)
        {

            var file = directory.GetFiles()
                .OrderByDescending(f => f.LastWriteTime)
                .First(x => x.Name.Contains(partOfFileName)).FullName;

            return file;
        }

        public static string GetFileWithName(this DirectoryInfo directory, string partOfFileName)
        {

            var file = directory.GetFiles()
                .OrderByDescending(f => f.LastWriteTime)
                .First(x => x.Name.Equals(partOfFileName)).FullName;

            return file;
        }

        public static bool FileWithNamePartExists(this DirectoryInfo directory, string partOfFileName)
        {
            try
            {
                return !string.IsNullOrEmpty(GetFileWithNamePart(directory, partOfFileName));
            }
            catch
            {
                return false;
            }
        }

        public static bool FileWithNameExists(this DirectoryInfo directory, string partOfFileName)
        {
            try
            {
                return !string.IsNullOrEmpty(GetFileWithName(directory, partOfFileName));
            }
            catch
            {
                return false;
            }
        }

        public static string WaitForFile(this DirectoryInfo directory, string fileName)
        {
            for (int i = 0; i < 15; i++)
            {
                if (FileWithNameExists(directory, fileName))
                    return GetFileWithName(directory,fileName);

                Thread.Sleep(3000);
            }

            throw new Exception($"File with '{fileName}' name was not found.");
        }

        public static string WaitForFileWithNamePart(this DirectoryInfo directory, string namePart)
        {
            for (int i = 0; i < 15; i++)
            {
                if (FileWithNameExists(directory, namePart))
                    return GetFileWithName(directory, namePart);

                Thread.Sleep(3000);
            }

            throw new Exception($"File with '{namePart}' name part was not found.");
        }
    }
}
