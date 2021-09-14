using System.Collections.Generic;
using System.Linq;

namespace AutomationUtils.Extensions
{
    public static class ListExtensions
    {
        public static string ToString(this List<string> list, string separator)
        {
            return list.Any() ? string.Join(separator, list.ToArray()) : string.Empty;
        }
    }
}
