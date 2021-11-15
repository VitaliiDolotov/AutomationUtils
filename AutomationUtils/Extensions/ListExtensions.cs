using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationUtils.Extensions
{
    public static class ListExtensions
    {
        public static string ToString(this IEnumerable<string> list, string separator)
        {
            var sourceList = list.ToList();
            return sourceList.Any() ? string.Join(separator, sourceList) : string.Empty;
        }

        public static T RandomItem<T>(this IEnumerable<T> list)
        {
            var listInstance = list.ToArray();
            var index = new Random().Next(listInstance.Count());
            return listInstance.ToArray()[index];
        }
    }
}
