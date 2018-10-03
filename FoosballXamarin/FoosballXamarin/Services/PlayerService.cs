using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FoosballXamarin.Models;
using FoosballXamarin.Models.Dtos;
using FoosballXamarin.Services;
using Models;
using Newtonsoft.Json;

[assembly: Xamarin.Forms.Dependency(typeof(PlayerService))]
namespace FoosballXamarin.Services
{
    public class PlayerService : BaseService, IPlayerService
    {
        public async Task<List<User>> GetDataAsync()
        {
            RestUrl = ApiUrl + "player/GetUsers";
            var messageBody = GetRequest(RestUrl, null, HttpMethod.Get);
            var response = await Client.SendAsync(messageBody);

            if (!response.IsSuccessStatusCode) return new List<User>();

            var content = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<List<User>>(content);
            return items;
        }

        public async Task<GetPlayerSeasonHistoryResponse> GetPlayerSeasonHistory(string email)
        {
            RestUrl = ApiUrl + $"player/GetPlayerHistory?email={email}";
            var response = await Client.GetAsync(RestUrl);

            if(!response.IsSuccessStatusCode) return new GetPlayerSeasonHistoryResponse();

            var content = await response.Content.ReadAsStringAsync();
            var item = JsonConvert.DeserializeObject<GetPlayerSeasonHistoryResponse>(content);
            return item;
        }

        public async Task<List<PartnerPercentResult>> GetPartnerPercentResult(string email)
        {
            RestUrl = ApiUrl + $"player/GetPlayerPartnerResults?email={email}";
            var response = await Client.GetAsync(RestUrl);

            if (!response.IsSuccessStatusCode) return new List<PartnerPercentResult>();

            var content = await response.Content.ReadAsStringAsync();
            var item = JsonConvert.DeserializeObject<List<PartnerPercentResult>> (content);
            return item;
        }

        public async Task<bool> CreateUser(string email, string displayName, string password)
        {
            var request = new CreateUserRequest
            {
                Email = email,
                Username = displayName,
                Password = password
            };

            RestUrl = ApiUrl + $"player/CreateUser";
            var messageBody = GetRequest(RestUrl, request, HttpMethod.Post);
            var response = await Client.SendAsync(messageBody);

            return response.IsSuccessStatusCode;
        }
    }
}