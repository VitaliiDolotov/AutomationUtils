using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AutomationUtils.Utils
{
    public static class ConfigReader
    {
        public static string ConfigFilePath = Directory.GetParent(Assembly.GetCallingAssembly().Location).GetFiles("appsettings.json").First().FullName;

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
