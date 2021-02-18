using System.Reflection;

namespace AutomationUtils.Utils
{
    public static class ConfigReader
    {
        public static string Read = Assembly.GetCallingAssembly().Location;
    }
}
