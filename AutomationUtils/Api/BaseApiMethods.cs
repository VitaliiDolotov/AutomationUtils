using RestSharp;

namespace AutomationUtils.Api
{
    public abstract class BaseApiMethods
    {
        private RestClient _client;

        public RestClient Client
        {
            protected get
            {
                return _client ??= new RestClient();
            }
            set
            {
                _client = value;
            }
        }
    }
}