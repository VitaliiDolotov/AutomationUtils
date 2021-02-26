using System.Reflection;

namespace AutomationUtils.Utils
{
    public static class Config
    {
        public static string Read = Assembly.GetCallingAssembly().Location;
    }
}
