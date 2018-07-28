using System.Collections.Generic;
using System.Threading.Tasks;
using FoosballXamarin.Models.Dtos;
using FoosballXamarin.Services;
using Models;
using Newtonsoft.Json;

[assembly: Xamarin.Forms.Dependency(typeof(UserService))]
namespace FoosballXamarin.Services
{
    public class UserService : BaseService, IUserService
    {
        public async Task<List<User>> GetDataAsync()
        {
            RestUrl = App.ApiUrl + "player/GetUsers";
            var response = await _client.GetAsync(RestUrl);

            if (!response.IsSuccessStatusCode) return new List<User>();

            var content = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<List<User>>(content);
            return items;
        }

        public async Task<GetPlayerSeasonHistoryResponse> GetPlayerSeasonHistory(string email)
        {
            RestUrl = App.ApiUrl + $"player/GetPlayerHistory?email={email}";
            var response = await _client.GetAsync(RestUrl);

            if(!response.IsSuccessStatusCode) return new GetPlayerSeasonHistoryResponse();

            var content = await response.Content.ReadAsStringAsync();
            var item = JsonConvert.DeserializeObject<GetPlayerSeasonHistoryResponse>(content);
            return item;
        }
    }
}