﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FoosballXamarin.Services;
using Models;
using Newtonsoft.Json;

[assembly: Xamarin.Forms.Dependency(typeof(LeaderboardService))]
namespace FoosballXamarin.Services
{
    public class LeaderboardService : BaseService, ILeaderboardService
    {
        public async Task<List<LeaderboardView>> GetDataAsync()
        {
            RestUrl = "http://foosball9000api.sovs.net/api/leaderboard/index";
            var response = await _client.GetAsync(HttpUri);

            if (!response.IsSuccessStatusCode) return new List<LeaderboardView>();

            var content = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<List<LeaderboardView>>(content);
            return items;
        }
    }
}