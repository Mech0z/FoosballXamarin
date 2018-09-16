using System;
using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FoosballXamarin.Models;
using FoosballXamarin.Models.Dtos;
using FoosballXamarin.Services;
using FoosballXamarin.UWP.Models.Dtos;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(MatchService))]
namespace FoosballXamarin.Services
{
    public class MatchService : BaseService, IMatchService
    {
        public async Task<List<Match>> GetDataAsync()
        {
            RestUrl = ApiUrl + "match/lastgames?numberofmatches=20"; 
            
            var messageBody = GetRequest(RestUrl, null, HttpMethod.Get);
            var response = await Client.SendAsync(messageBody);

            if (!response.IsSuccessStatusCode) return new List<Match>();

            var content = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<List<Match>>(content);
            return items;
        }

        public async Task<List<Match>> GetPlayerMatches(string email)
        {
            RestUrl = ApiUrl + $"player/GetPlayerMatches?email={email}";
            var response = await Client.GetAsync(HttpUri);

            if (!response.IsSuccessStatusCode) return new List<Match>();

            var content = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<List<Match>>(content);
            return items;
        }

        public async Task<bool> SubmitMatches(SaveMatchesRequest request)
        {
            RestUrl = ApiUrl + "match/SaveMatch";
            var serilizedUserSettings = Preferences.Get("UserSettings", "");
            var userSettings = JsonConvert.DeserializeObject<UserSettings>(serilizedUserSettings);
            request.Email = userSettings.Email;
            var httpRequestMessage = GetRequest(RestUrl, request, HttpMethod.Post);

            var response = await Client.SendAsync(httpRequestMessage);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                return false;
            }
            else
            {
                Console.WriteLine("added match");
                return true;
            }
        }
    }
}