using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FoosballXamarin.Services
{
    public class BaseService
    {
        HttpClient client;
        
        public string RestUrl { get; set; }
        public Uri HttpUri => new Uri(string.Format(RestUrl, string.Empty));

        public BaseService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;

            RestUrl = "http://foosball9000api.sovs.net/api/match/lastgames?numberofmatches=20";
        }
    }

    public class MatchService : BaseService, IMatchService
    {

        public MatchService()
        {
        }

        public async Task<List<Match>> RefreshDataAsync()
        {
            var response = await client.GetAsync(HttpUri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<List<Match>>(content);
                return items;
            }
            else
            {
                return new List<Match>();
            }
        }
    }
}