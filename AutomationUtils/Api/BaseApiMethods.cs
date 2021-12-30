using RestSharp;

namespace AutomationUtils.Api
{
    interface IBaseApiMethods
    {
        RestClient Client { set; }
    }

    public abstract class BaseApiMethods : IBaseApiMethods
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