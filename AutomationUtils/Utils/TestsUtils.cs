using AutomationUtils.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutomationUtils.Utils
{
    public class TestsUtils
    {
        static TestsUtils()
        {
            CheckAllFilesAndAddData(AllFilesNames);
        }

        public static readonly List<KeyValuePair<string, List<string>>> TestsAndTags = new();
        private static readonly string SourceFolder = SolutionDirectoryInfo().FullName;
        private const string Extension = "*.feature";
        private const string ScenarioKeyword = "Scenario";
        private static readonly Regex SearchWord = new($@"{ScenarioKeyword}\s*(\w*):\s*");
        private static readonly List<string> AllFileNames = new();
        private static List<string> AllFilesNames { get; } = AddFileNamesToList(SourceFolder, Extension, AllFileNames);
        public static Dictionary<string, List<string>> FeatureFilesAndTheirContent = AllFeatureFilesAndTheirContent();

        private static DirectoryInfo SolutionDirectoryInfo()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }

            return directory;
        }

        private static List<string> AddFileNamesToList(string sourceDir, string extension, List<string> allFiles)
        {
            var fileEntries = Directory.GetFiles(sourceDir, extension);
            allFiles.AddRange(fileEntries);

            // Recursion
            var subDirectoryEntries = Directory.GetDirectories(sourceDir);
            foreach (var item in subDirectoryEntries)
            {
                // Avoid "reparse points"
                if ((File.GetAttributes(item) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                {
                    AddFileNamesToList(item, extension, allFiles);
                }
            }

            return allFiles;
        }

        private static void CheckAllFilesAndAddData(List<string> allFiles)
        {
            foreach (var fileName in allFiles)
            {
                AddTestAndTagsToList(new List<string>(), File.ReadLines(fileName));
            }
        }

        private static void AddTestAndTagsToList(List<string> tagList, IEnumerable<string> fileLines)
        {
            var enumerable = fileLines.ToList();
            string result = null;
            for (var i = 0; i < enumerable.Count; i++)
            {
                var lineTrim = enumerable[i].Trim();
                if (lineTrim.StartsWith("@"))
                {
                    tagList.AddRange(lineTrim.Replace("@", "").Split(" "));
                }

                if (SearchWord.IsMatch(lineTrim) && lineTrim.StartsWith(ScenarioKeyword))
                {
                    result = lineTrim
                        .Substring(lineTrim.IndexOf(SearchWord.Match(enumerable[i]).Value, StringComparison.Ordinal) +
                                   SearchWord.Match(lineTrim).Value.Length);

                    if (lineTrim.Contains("Outline"))
                    {
                        continue;
                    }

                    TestsAndTags.Add(new KeyValuePair<string, List<string>>(result, tagList));
                    tagList = new List<string>();
                }

                // Add to test name part from examples table
                if (lineTrim.Contains("Examples"))
                {
                    // Move on the next line after 'Examples'
                    i++;

                    // Skip all lines with comments or line breaks in the example table before the first line between '|' chars
                    i = SkipLineBreaksAndCommentsInExamplesTable(enumerable, i);

                    // Skip variable names line 
                    i++;

                    while (enumerable[i].Contains("|"))
                    {
                        var example = enumerable[i].Trim().GetTextBetween('|', '|', false).First().Trim();
                        var testName = string.Concat(result, ", ", example);
                        TestsAndTags.Add(new KeyValuePair<string, List<string>>(testName, tagList));

                        if (i == enumerable.Count - 1)
                        {
                            break;
                        }

                        i++;

                        i = SkipLineBreaksAndCommentsInExamplesTable(enumerable, i);
                    }

                    i--;
                    tagList = new List<string>();
                }
            }
        }

        private static int SkipLineBreaksAndCommentsInExamplesTable(List<string> fileLines, int iterator)
        {
            while (fileLines[iterator].Equals(string.Empty)
                   || fileLines[iterator].Replace("\t", string.Empty).StartsWith("#"))
            {
                iterator++;
            }

            return iterator;
        }

        private static Dictionary<string, List<string>> AllFeatureFilesAndTheirContent()
        {
            Dictionary<string, List<string>> dictionary = new();

            foreach (var fileName in AllFilesNames)
            {
                FileStream fileStream = new(fileName, FileMode.Open);
                StreamReader streamReader = new(fileStream);
                var fileLines = streamReader.ReadToEnd().Split(Environment.NewLine).ToList();

                dictionary.Add(fileName, fileLines);
            }

            return dictionary;
        }
    }
}