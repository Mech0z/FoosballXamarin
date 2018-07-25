using System;
using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FoosballXamarin.Services;
using FoosballXamarin.UWP.Models.Dtos;
using Plugin.DeviceInfo;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(MatchService))]
namespace FoosballXamarin.Services
{
    public class MatchService : BaseService, IMatchService
    {
        public async Task<List<Match>> GetDataAsync()
        {
            RestUrl = App.ApiUrl + "match/lastgames?numberofmatches=20";
            var response = await _client.GetAsync(HttpUri);

            if (!response.IsSuccessStatusCode) return new List<Match>();

            var content = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<List<Match>>(content);
            return items;
        }

        public async Task<List<Match>> GetPlayerMatches(string email)
        {
            RestUrl = App.ApiUrl + $"player/GetPlayerMatches?email={email}";
            var response = await _client.GetAsync(HttpUri);

            if (!response.IsSuccessStatusCode) return new List<Match>();

            var content = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<List<Match>>(content);
            return items;
        }

        public async Task<bool> SubmitMatches(SaveMatchesRequest request)
        {
            var email = Application.Current.Properties["Email"] as string;
            request.Email = email;

            RestUrl = App.ApiUrl + "match/SaveMatch";
            
            var jsonRequest = JsonConvert.SerializeObject(request);
            string contentType = "application/json";

            var response = await _client.PostAsync(HttpUri, new StringContent(jsonRequest, Encoding.UTF8, contentType));

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