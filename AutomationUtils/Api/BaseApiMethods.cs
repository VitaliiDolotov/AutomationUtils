using RestSharp;

namespace AutomationUtils.Api
{
    public abstract class BaseApiMethods
    {
        public RestClient Client { protected get; set; }
    }
}