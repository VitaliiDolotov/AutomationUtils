using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AutomationUtils.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsText(this string fullText, string term)
        {
            CultureInfo culture = CultureInfo.InvariantCulture;
            return culture.CompareInfo.IndexOf(fullText, term, CompareOptions.IgnoreCase) >= 0;
        }

        private static readonly Regex SpaceTrimmer = new Regex(@"\s\s+");

        public static string RemoveExtraSpaces(this string str)
        {
            try
            {
                return SpaceTrimmer.Replace(str, " ");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<string> SplitByLinebraeak(this string str)
        {
            return str.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
        }

        public static List<string> Split(this string str, string separator)
        {
            return str.Split(new string[] { separator }, StringSplitOptions.None).ToList();
        }

        public static List<string> Split(this string str, char separator, params char[] additionalSeparators)
        {
            var separators = new List<char> { separator };
            if (additionalSeparators.Any())
            {
                separators.AddRange(additionalSeparators);
            }
            return str.Split(separators.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static DateTime GetNextWeekday(this string dayOfWeek)
        {
            var day = EnumExtensions.Parse<DayOfWeek>(dayOfWeek);
            //to get the value for "today or in the next 6 days"
            var start = DateTime.Today.AddDays(1);
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        public static string RemoveBracketsText(this string str)
        {
            Regex regex = new Regex(@"\(.*?\)");
            string cleanString = regex.Replace(str, String.Empty).TrimEnd(' ');
            return cleanString;
        }

        public static string GetBracketsValue(this string str)
        {
            Regex regex = new Regex(@"(?<=\()(.*)(?=\))");
            Match m = regex.Match(str);
            var match = m.Value;
            return match;
        }

        public static string ReadJsonProperty(this string str, string property)
        {
            var responseContent = JsonConvert.DeserializeObject<JObject>(str);
            var content = responseContent[property].ToString();
            return content;
        }

        public static string RemoveLineBreaks(this string str)
        {
            return str.Replace(Environment.NewLine, string.Empty)
                .Replace("\r\n", String.Empty)
                .Replace("\r", string.Empty)
                .Replace("\n", string.Empty);
        }

        /// <summary>
        /// Used for appsettings file reading
        /// </summary>
        /// <param name="assemblyPath">Assembly Path</param>
        /// <param name="key">Config Key</param>
        /// <returns></returns>
        public static string ByKey(this string assemblyPath, string key)
        {
            var path = Directory.GetParent(assemblyPath).GetFiles("appsettings.json").First().FullName;
            using (StreamReader sr = new StreamReader(path))
            {
                try
                {

                    string configFileContent = sr.ReadToEnd();

                    var responseContent = JsonConvert.DeserializeObject<JObject>(configFileContent);
                    string value = responseContent[key].ToString();

                    return value;
                }
                catch (Exception e)
                {
                    throw new Exception($"Unable to read configuration property for '{key}' key: {e}");
                }
            }
        }

        public static List<string> GetTextBetween(this string source, string firstPart, string secondPart, bool textBeforeLastOccurrenceOfSecondString = true)
        {
            var pattern = textBeforeLastOccurrenceOfSecondString ?
                @"(?<=" + firstPart + @")(.*)(?=" + secondPart + ")" :
                @"(?<=" + firstPart + ").+?(?=" + secondPart + ")";
            var matches = Regex.Matches(source, pattern);
            var results = new List<string>();
            foreach (Match match in matches)
            {
                results.Add(match.Value);
            }
            return results;
        }

        public static List<string> GetTextBetween(this string source, char characters, bool includeChars)
        {
            return GetTextBetweenTwoCharacters(source, characters, characters, includeChars);
        }

        public static List<string> GetTextBetween(this string source, char firstChar, char secondChar, bool includeChars)
        {
            return GetTextBetweenTwoCharacters(source, firstChar, secondChar, includeChars);
        }

        private static List<string> GetTextBetweenTwoCharacters(string source, char firstChar, char secondChar, bool includeChars)
        {
            string pattern = includeChars ?
                @"\" + firstChar + @"(.*?)\" + secondChar :
                @"(?<=\" + firstChar + @")(.*?)(?=\" + secondChar + ")";
            MatchCollection matches = Regex.Matches(source, pattern);
            List<string> results = new List<string>();
            foreach (Match match in matches)
            {
                results.Add(match.Value);
            }
            return results;
        }

        public static int GetNumber(this string str)
        {
            return int.Parse(Regex.Match(str, @"\d+").Value);
        }

        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => input,
                _ => $"{char.ToUpper(input[0])}{input[1..]}"
            };

        public static string RemoveAfter(this string value, string removeAfter)
        {
            var index = value.IndexOf(removeAfter);
            if (index >= 0)
            {
                value = value.Remove(index);
            }

            return value;
        }
    }
}
