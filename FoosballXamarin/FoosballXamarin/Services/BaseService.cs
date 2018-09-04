using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using FoosballXamarin.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace FoosballXamarin.Services
{
    public class BaseService
    {
        protected readonly HttpClientWrapper Client;
        
        public string RestUrl { get; set; }
        public Uri HttpUri => new Uri(string.Format(RestUrl, string.Empty));
        public string ApiUrl => Preferences.Get("ApiUrlSettings", "");

        public BaseService()
        {
            Client = new HttpClientWrapper
            {
                //MaxResponseContentBufferSize = 256000
            };
        }

        public HttpRequestMessage GetRequest(string uri, object bodyContent, HttpMethod httpMethod)
        {
            var serilizedUserSettings = Preferences.Get("UserSettings", "");
            var userSettings = JsonConvert.DeserializeObject<UserSettings>(serilizedUserSettings);
            var deviceName = DeviceInfo.Name;
            
            var jsonRequest = JsonConvert.SerializeObject(bodyContent);
            var contentType = "application/json";

            HttpRequestMessage httpRequest;
            if (bodyContent != null)
            {
                httpRequest = new HttpRequestMessage(httpMethod, RestUrl)
                {
                    Content = new StringContent(jsonRequest, Encoding.UTF8, contentType)
                };
            }
            else
            {
                httpRequest = new HttpRequestMessage(httpMethod, RestUrl)
                {
                };
            }

            if (userSettings != null)
            {
                httpRequest.Headers.Add("Email", new List<string> {userSettings.Email});
                httpRequest.Headers.Add("Token", new List<string> {userSettings.Token});
                httpRequest.Headers.Add("DeviceName", new List<string> {deviceName});
            }

            return httpRequest;
        }
    }
}