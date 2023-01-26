using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RestSharp;

namespace AutomationUtils.Extensions
{
    public static class ResponseExtensions
    {
        public static void Validate(this RestResponse response, HttpStatusCode expectedCode, string exceptionMessage, params HttpStatusCode[] expectedCodes)
        {
            var codes = new List<HttpStatusCode>();
            codes.AddRange(expectedCodes);
            codes.Add(expectedCode);
            if (!codes.Contains(response.StatusCode))
            {
                var details = new List<string>() { response.StatusCode.ToString(), response.ErrorMessage, response.Content };
                throw new Exception($"{exceptionMessage}: {string.Join(", ", details.Where(x => !string.IsNullOrEmpty(x)))}");
            }
        }
    }
}
