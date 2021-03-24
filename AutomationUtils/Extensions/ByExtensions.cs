using System.Linq;
using OpenQA.Selenium;

namespace AutomationUtils.Extensions
{
    public static class ByExtensions
    {
        public static string Selector(this By @by)
        {
            var selector = @by.ToString().Split(": ").Last();
            return selector;
        }
    }
}
