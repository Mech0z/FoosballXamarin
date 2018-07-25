using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Plugin.DeviceInfo;
using Xamarin.Forms;

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

        public HttpRequestMessage GetRequest(string uri, object bodyContent)
        {
            var token = Application.Current.Properties.ContainsKey("Token") ? Application.Current.Properties["Token"] as string : "";
            var email = Application.Current.Properties.ContainsKey("Email") ? Application.Current.Properties["Email"] as string : "";
            var deviceName = CrossDeviceInfo.Current.DeviceName;

            var jsonRequest = JsonConvert.SerializeObject(bodyContent);
            var contentType = "application/json";

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, RestUrl)
            {
                Content = new StringContent(jsonRequest, Encoding.UTF8, contentType)
            };
            httpRequest.Headers.Add("Email", new List<string> { email });
            httpRequest.Headers.Add("Token", new List<string> { token });
            httpRequest.Headers.Add("DeviceName", new List<string> { deviceName });

            return httpRequest;
        }
    }
}