using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FoosballXamarin.Models;
using FoosballXamarin.Services;
using Newtonsoft.Json;

[assembly: Xamarin.Forms.Dependency(typeof(LeaderboardService))]
namespace FoosballXamarin.Services
{
    public class LeaderboardService : BaseService, ILeaderboardService
    {
        public async Task<List<LeaderboardView>> GetDataAsync()
        {
            RestUrl = ApiUrl + "leaderboard/index";
            var messageBody = GetRequest(RestUrl, null, HttpMethod.Get);
            var response = await Client.SendAsync(messageBody);

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<List<LeaderboardView>>(content);
            return items;
        }
    }
}