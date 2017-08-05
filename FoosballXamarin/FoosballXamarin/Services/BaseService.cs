using System;
using System.Net.Http;

namespace FoosballXamarin.Services
{
    public class BaseService
    {
        protected readonly HttpClient _client;
        
        public string RestUrl { get; set; }
        public Uri HttpUri => new Uri(string.Format(RestUrl, string.Empty));

        public BaseService()
        {
            _client = new HttpClient
            {
                //MaxResponseContentBufferSize = 256000
            };
        }
    }
}