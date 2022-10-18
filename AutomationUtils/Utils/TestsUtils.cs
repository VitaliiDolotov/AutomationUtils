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
        private const string _extension = "*.feature";
        private const string _scenarioKeyword = "Scenario";

        private static readonly string _sourceFolder = SolutionDirectoryInfo().FullName;
        private static readonly Regex _searchWord = new($@"{_scenarioKeyword}\s*(\w*):\s*");
        private static readonly List<string> _allFileNames = new();

        public static readonly List<KeyValuePair<string, List<string>>> TestsAndTags = new();
        public static readonly Dictionary<string, List<string>> FeatureFilesAndTheirContent = new();

        static TestsUtils()
        {
            _allFileNames = AddFileNamesToList(_sourceFolder, _extension, _allFileNames);
            FeatureFilesAndTheirContent = AllFeatureFilesAndTheirContent();

            foreach (var file in FeatureFilesAndTheirContent)
            {
                AddTestAndTagsToList(new List<string>(), file.Value);
            }
        }

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

                if (_searchWord.IsMatch(lineTrim) && lineTrim.StartsWith(_scenarioKeyword))
                {
                    result = lineTrim
                        .Substring(lineTrim.IndexOf(_searchWord.Match(fileLines[i]).Value, StringComparison.Ordinal) +
                                   _searchWord.Match(lineTrim).Value.Length);

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

            foreach (var fileName in _allFileNames)
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