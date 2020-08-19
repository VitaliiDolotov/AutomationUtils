using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AutomationUtils.Utils
{
    public static class TestDataGenerator
    {
        private static readonly Random Generator = new Random();

        private static int _randomNumber;

        public static string RandomString(int length = 6)
        {
            var baseString = Guid.NewGuid().ToString("N");
            while (baseString.Length <= length)
            {
                baseString = Guid.NewGuid().ToString("N");
            }

            return baseString.Substring(0, length);
        }

        public static string RandomEmail()
        {
            return $"At_{RandomString()}@attest.com";
        }

        /// <summary>
        /// This method handle situation when same random numbers are generated instantly
        /// </summary>
        /// <param name="maxValue"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public static int RandomNum(int maxValue, int minValue = 0)
        {
            int randNum = new Random().Next(minValue, maxValue);
            int attempts = 5;
            while (randNum == _randomNumber && attempts > 0)
            {
                randNum = new Random().Next(maxValue);
                attempts--;

                if (randNum == _randomNumber)
                    System.Threading.Thread.Sleep(14);
            }
            _randomNumber = randNum;
            return randNum;
        }

        public static string ValidPhoneNumber(int length)
        {
            const string numbers = "0123456789";
            string phone = "";

            while (true)
            {
                phone = new string(Enumerable.Repeat(numbers, length)
                    .Select(s => s[Generator.Next(s.Length)]).ToArray());

                if (Regex.IsMatch(phone, @"^(\+?1)?[2-9]\d{2}[2-9](?!11)\d{6}", RegexOptions.IgnoreCase))
                {
                    Console.WriteLine($"Phone number {phone} is correct");
                    break;
                }
                else
                {
                    Console.WriteLine($"Phone number {phone} is invalid");
                }
            }
            return phone;
        }

        public static string ValidPhoneNumber(string countryCode, int length)
        {
            return $"{countryCode}{TestDataGenerator.ValidPhoneNumber(length)}";
        }
    }
}
