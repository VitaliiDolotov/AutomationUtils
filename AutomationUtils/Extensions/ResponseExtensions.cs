using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;

namespace AutomationUtils.Extensions
{
    public static class ResponseExtensions
    {
        public static void Validate(this IRestResponse response, HttpStatusCode expectedCode, string exceptionMessage, params HttpStatusCode[] expectedCodes)
        {
            var codes = new List<HttpStatusCode>();
            codes.AddRange(expectedCodes);
            codes.Add(expectedCode);
            if (!codes.Contains(response.StatusCode))
            {
                throw new Exception($"{exceptionMessage}: {string.Join(", ", response.StatusCode, response.ErrorMessage, response.Content)}");
            }
        }
    }
}
