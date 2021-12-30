using AutomationUtils.Api;
using RestSharp;

namespace AutomationUtils.Extensions
{
    public static class RestClientExtensions
    {
        public static T InitApiMethods<T>(this RestClient client) where T : IBaseApiMethods, new()
        {
            var requestMethods = new T { Client = client };
            return requestMethods;
        }
    }
}
