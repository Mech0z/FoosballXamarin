using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FoosballXamarin.Models;
using FoosballXamarin.Models.Dtos;
using FoosballXamarin.Services;
using FoosballXamarin.UWP.Models.Dtos;
using Newtonsoft.Json;
using Plugin.DeviceInfo;
using Xamarin.Forms;

[assembly: Dependency(typeof(AdministrationService))]
namespace FoosballXamarin.Services
{
    public class AdministrationService : BaseService, IAdministrationService
    {
        public async Task<List<UserMapping>> GetUsermappings()
        {
            if (!Application.Current.Properties.ContainsKey("Token")) return new List<UserMapping>();
            
            var token = Application.Current.Properties["Token"] as string;
            var email = Application.Current.Properties["Email"] as string;

            var deviceName = CrossDeviceInfo.Current.DeviceName;
            var request = new BaseRequest
            {
                Email = email,
                Token = token,
                DeviceName = deviceName
            };

            RestUrl = App.ApiUrl + "Administration/GetUserMappings";
            
            var jsonRequest = JsonConvert.SerializeObject(request);
            var contentType = "application/json";

            var response = await _client.PostAsync(HttpUri, new StringContent(jsonRequest, Encoding.UTF8, contentType));
         
            if (!response.IsSuccessStatusCode) return new List<UserMapping>();

            var content = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<GetUserMappingsResponse>(content);
            
            return deserializedResponse.Users.Select(x => new UserMapping(x)).ToList();
        }

        public async Task<bool> ChangeUserPassword(string userEmail, string newPassword)
        {
            if (!Application.Current.Properties.ContainsKey("Token")) return false;
            
            var token = Application.Current.Properties["Token"] as string;
            var email = Application.Current.Properties["Email"] as string;

            var deviceName = CrossDeviceInfo.Current.DeviceName;
            var request = new ChangeUserPasswordRequest
            {
                Email = email,
                Token = token,
                DeviceName = deviceName,
                NewPassword = newPassword,
                UserEmail = userEmail
            };

            RestUrl = App.ApiUrl + "Administration/ChangeUserPassword";
            
            var jsonRequest = JsonConvert.SerializeObject(request);
            var contentType = "application/json";

            var response = await _client.PostAsync(HttpUri, new StringContent(jsonRequest, Encoding.UTF8, contentType));
         
            if (!response.IsSuccessStatusCode) return false;

            var content = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(content);
            
            return deserializedResponse;
        }

        public async Task<bool> ChangeUserRoles(string userEmail, List<string> newUserRoles)
        {
            if (!Application.Current.Properties.ContainsKey("Token")) return false;
            
            var token = Application.Current.Properties["Token"] as string;
            var email = Application.Current.Properties["Email"] as string;

            var deviceName = CrossDeviceInfo.Current.DeviceName;
            var request = new ChangeUserRolesRequest
            {
                Email = email,
                Token = token,
                DeviceName = deviceName,
                UserEmail = userEmail,
                NewUserRoles = newUserRoles
            };

            RestUrl = App.ApiUrl + "Administration/ChangeUserRoles";
            
            var jsonRequest = JsonConvert.SerializeObject(request);
            var contentType = "application/json";

            var response = await _client.PostAsync(HttpUri, new StringContent(jsonRequest, Encoding.UTF8, contentType));
         
            if (!response.IsSuccessStatusCode) return false;

            var content = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(content);
            
            return deserializedResponse;
        }
    }
}