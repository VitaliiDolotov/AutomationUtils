using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using AutomationUtils.Utils;

namespace AutomationUtils.Extensions
{
    public static class EnumExtensions
    {
        public static string GetValue(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            return value.ToString();
        }

        public static KeyValuePair<string, string> GetValueAndDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            var result = new KeyValuePair<string, string>(value.ToString(),
                attributes.Length > 0 ? attributes[0].Description : value.ToString());
            return result;
        }

        // Usage EnumExtensions.Parse<YourEnum>()
        public static T Parse<T>(string enumValue) where T : Enum
        {
            var result = EnumExtensions.GetAllValues<T>().First(x =>
                x.GetValueAndDescription().Key.Equals(enumValue) || x.GetValueAndDescription().Value.Equals(enumValue));
            return result;
        }

        /// <summary>
        /// Usage EnumExtensions.GetAllValues<YourEnum>()
        /// </summary>
        public static IEnumerable<T> GetAllValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static T GetRandomValue<T>()
        {
            MemberInfo type = typeof(T);

            object randomValue = (T)Enum.ToObject(typeof(T), TestDataGenerator.RandomNum(Enum.GetNames(typeof(T)).Length));
            Logger.Write("Random value from <{0}> Enum is: '{1}'", type.Name, ((Enum)randomValue).GetValue());
            return (T)randomValue;
        }

        /// <summary>
        /// Get random value from appropriate Enum
        /// Example: EnumExtensions.GetRandomValue&gt;ServiceLevelLiteralEnum&gt;(1,2,3)
        /// </summary>
        /// <typeparam name="T">Enum Type</typeparam>
        /// <param name="fieldNumbers">Starts from 0</param>
        /// <returns></returns>
        public static T GetRandomValue<T>(params int[] fieldNumbers)
        {
            MemberInfo type = typeof(T);

            object randomValue = (T)Enum.ToObject(typeof(T), fieldNumbers[TestDataGenerator.RandomNum(fieldNumbers.Length)]);
            Logger.Write("Random value from <{0}> Enum is: '{1}'", type.Name, ((Enum)randomValue).GetValue());
            return (T)randomValue;
        }
    }
}
