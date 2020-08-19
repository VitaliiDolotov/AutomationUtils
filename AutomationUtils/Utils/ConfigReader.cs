using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AutomationUtils.Utils
{
    public static class ConfigReader
    {
        private static string ConfigFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "appsettings.json");

        public static string ByKey(string key)
        {
            using (StreamReader sr = new StreamReader(ConfigFilePath))
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
    }
}
