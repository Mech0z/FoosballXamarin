using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoosballXamarin.Models;
using FoosballXamarin.Models.Dtos;
using FoosballXamarin.Services;
using FoosballXamarin.UWP.Models.Dtos;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(AdministrationService))]
namespace FoosballXamarin.Services
{
    public class AdministrationService : BaseService, IAdministrationService
    {
        public async Task<List<UserMapping>> GetUsermappings()
        {
            if (!Preferences.ContainsKey("UserSettings")) return new List<UserMapping>();
            
            RestUrl = App.ApiUrl + "Administration/GetUserMappings";

            var messageBody = GetRequest(RestUrl, "");

            var response = await _client.SendAsync(messageBody);
         
            if (!response.IsSuccessStatusCode) return new List<UserMapping>();

            var content = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<GetUserMappingsResponse>(content);
            
            return deserializedResponse.Users.Select(x => new UserMapping(x)).ToList();
        }

        public async Task<bool> ChangeUserPassword(string userEmail, string newPassword)
        {
            if (!Preferences.ContainsKey("UserSettings")) return false;
            
            var request = new ChangeUserPasswordRequest
            {
                NewPassword = newPassword,
                UserEmail = userEmail
            };

            RestUrl = App.ApiUrl + "Administration/ChangeUserPassword";

            var messageBody = GetRequest(RestUrl, request);

            var response = await _client.SendAsync(messageBody);
         
            if (!response.IsSuccessStatusCode) return false;

            var content = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(content);
            
            return deserializedResponse;
        }

        public async Task<bool> ChangeUserRoles(string userEmail, List<string> newUserRoles)
        {
            if (!Preferences.ContainsKey("UserSettings")) return false;
            
            var request = new ChangeUserRolesRequest
            {
                UserEmail = userEmail,
                Roles = newUserRoles
            };

            RestUrl = App.ApiUrl + "Administration/ChangeUserRoles";

            var messageBody = GetRequest(RestUrl, request);

            var response = await _client.SendAsync(messageBody);
         
            if (!response.IsSuccessStatusCode) return false;

            var content = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(content);
            
            return deserializedResponse;
        }

        public async Task<bool> StartNewSeason()
        {
            if (!Preferences.ContainsKey("UserSettings")) return false;

            RestUrl = App.ApiUrl + "SeasonsAdministration/StartNewSeason";

            var messageBody = GetRequest(RestUrl, "");

            var response = await _client.SendAsync(messageBody);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<Season>> GetSeasons()
        {
            if (!Preferences.ContainsKey("UserSettings")) return new List<Season>();

            RestUrl = App.ApiUrl + "Administration/GetSeasons";

            var messageBody = GetRequest(RestUrl, "");

            var response = await _client.SendAsync(messageBody);

            if (!response.IsSuccessStatusCode) return new List<Season>();

            var content = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<List<Season>>(content);

            return deserializedResponse;
        }
    }
}