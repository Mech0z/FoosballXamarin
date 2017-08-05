using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FoosballXamarin.Services;

[assembly: Xamarin.Forms.Dependency(typeof(MatchService))]
namespace FoosballXamarin.Services
{
    public class MatchService : BaseService, IMatchService
    {

        public MatchService() : base("http://foosball9000api.sovs.net/api/match/lastgames?numberofmatches=20")
        {
        }

        public async Task<List<Match>> GetDataAsync()
        {
            var response = await _client.GetAsync(HttpUri);

            if (!response.IsSuccessStatusCode) return new List<Match>();

            var content = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<List<Match>>(content);
            return items;
        }
    }
}