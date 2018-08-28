using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FoosballXamarin.Models;
using FoosballXamarin.Services;
using FoosballXamarin.UWP.Models.Dtos;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoginService))]
namespace FoosballXamarin.Services
{
    public class LoginService : BaseService, ILoginService
    {
        public async Task<bool> Login(string email, string password, bool rememberMe)
        {
            RestUrl = ApiUrl + "Account/Login";

            var deviceName = DeviceInfo.Name;
            var request = new LoginRequest
            {
                Email = email,
                Password = password,
                RememberMe = rememberMe,
                DeviceName = deviceName
            };

            var messageBody = GetRequest(RestUrl, request, HttpMethod.Post);

            var response = await _client.SendAsync(messageBody);
         
            if (!response.IsSuccessStatusCode) return false;

            var content = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<LoginResponse>(content);

            var userSettings = new UserSettings
            {
                Email = email,
                ExpiryTime = deserializedResponse.ExpiryTime,
                Roles = deserializedResponse.Roles,
                Token = deserializedResponse.Token
            };
            var serializeObject = JsonConvert.SerializeObject(userSettings);
            Preferences.Set("UserSettings", serializeObject);

            return !deserializedResponse.LoginFailed;
        }

        public async Task<bool> ValidateLogin()
        {
            if (!Preferences.ContainsKey("UserSettings")) return false;

            RestUrl = ApiUrl + "Account/ValidateLogin";

            var messageBody = GetRequest(RestUrl, "", HttpMethod.Post);

            var response = await _client.SendAsync(messageBody);
         
            if (!response.IsSuccessStatusCode) return false;

            var content = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<LoginResponse>(content);

            if (!deserializedResponse.LoginFailed)
            {
                var serilizedUserSettings = Preferences.Get("UserSettings", "");
                var userSettings = JsonConvert.DeserializeObject<UserSettings>(serilizedUserSettings);
                userSettings.ExpiryTime = deserializedResponse.ExpiryTime;
                var serializedObject = JsonConvert.SerializeObject(userSettings);
                Preferences.Set("UserSettings", serializedObject);
            }

            return !deserializedResponse.LoginFailed;
        }

        public async Task<bool> Logout()
        {
            if (!Preferences.ContainsKey("UserSettings")) return false;
            
            RestUrl = ApiUrl + "Account/Logout";

            var messageBody = GetRequest(RestUrl, "", HttpMethod.Post);
            
            var response = await _client.SendAsync(messageBody);
         
            if (!response.IsSuccessStatusCode) return false;

            var content = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(content);

            Preferences.Clear();

            return deserializedResponse;
        }
    }
}