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
            AllFilesNames = AddFileNamesToList(SourceFolder, Extension, AllFilesNames);
            FeatureFilesAndTheirContent = AllFeatureFilesAndTheirContent();

            foreach (var file in FeatureFilesAndTheirContent)
            {
                AddTestAndTagsToList(new List<string>(), file.Value);
            }
        }

        private static readonly string SourceFolder = SolutionDirectoryInfo().FullName;
        private const string Extension = "*.feature";
        private const string ScenarioKeyword = "Scenario";
        private static readonly Regex SearchWord = new($@"{ScenarioKeyword}\s*(\w*):\s*");
        private static readonly List<string> AllFilesNames = new();

        public static readonly List<KeyValuePair<string, List<string>>> TestsAndTags = new();
        public static Dictionary<string, List<string>> FeatureFilesAndTheirContent = new();

        private static DirectoryInfo SolutionDirectoryInfo()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (directory is not null && !directory.GetFiles("*.sln").Any())
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

        private static void AddTestAndTagsToList(List<string> tagList, List<string> fileLines)
        {
            string result = null;
            for (var i = 0; i < fileLines.Count; i++)
            {
                var lineTrim = fileLines[i].Trim();
                if (lineTrim.StartsWith("@"))
                {
                    tagList.AddRange(lineTrim.Replace("@", "").Split(" "));
                }

                if (SearchWord.IsMatch(lineTrim) && lineTrim.StartsWith(ScenarioKeyword))
                {
                    result = lineTrim
                        .Substring(lineTrim.IndexOf(SearchWord.Match(fileLines[i]).Value, StringComparison.Ordinal) +
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
                    i = SkipLineBreaksAndCommentsInExamplesTable(fileLines, i);

                    // Skip variable names line 
                    i++;

                    while (fileLines[i].Contains("|"))
                    {
                        var example = fileLines[i].Trim().GetTextBetween('|', '|', false).First().Trim();
                        var testName = string.Concat(result, ", ", example);
                        TestsAndTags.Add(new KeyValuePair<string, List<string>>(testName, tagList));

                        if (i == fileLines.Count - 1)
                        {
                            break;
                        }

                        i++;

                        i = SkipLineBreaksAndCommentsInExamplesTable(fileLines, i);
                    }

                    i--;
                    tagList = new List<string>();
                }
            }
        }

        private static int SkipLineBreaksAndCommentsInExamplesTable(List<string> fileLines, int iterator)
        {
            while (iterator < fileLines.Count - 1 && (fileLines[iterator].Equals(string.Empty)
                   || fileLines[iterator].Replace("\t", string.Empty).StartsWith("#")))
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
                using FileStream fileStream = new(fileName, FileMode.Open);
                using StreamReader streamReader = new(fileStream);
                var fileLines = streamReader
                    .ReadToEnd()
                    .Split("\n")
                    .Select(x => x.TrimEnd('\r'))
                    .ToList();

                dictionary.Add(fileName, fileLines);
            }

            return dictionary;
        }
    }
}