using RestSharp;

namespace GravitAutomationCore.Base
{
    public abstract class BaseApiMethods
    {
        public RestClient Client { protected get; set; }
    }
}