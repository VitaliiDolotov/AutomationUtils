﻿
using System;
using System.Collections.Generic;
using System.Globalization;
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
    }
}
