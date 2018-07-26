using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FoosballXamarin.Services;
using FoosballXamarin.UWP.Models.Dtos;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoginService))]
namespace FoosballXamarin.Services
{
    public class LoginService : BaseService, ILoginService
    {
        public async Task<bool> Login(string email, string password, bool rememberMe)
        {
            var deviceName = "App";
            var request = new LoginRequest
            {
                Email = email,
                Password = password,
                RememberMe = rememberMe,
                DeviceName = deviceName
            };

            RestUrl = App.ApiUrl + "Account/Login";
            
            var jsonRequest = JsonConvert.SerializeObject(request);
            var contentType = "application/json";

            var response = await _client.PostAsync(HttpUri, new StringContent(jsonRequest, Encoding.UTF8, contentType));
         
            if (!response.IsSuccessStatusCode) return false;

            var content = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<LoginResponse>(content);

            Application.Current.Properties["Email"] = email;
            Application.Current.Properties["Token"] = deserializedResponse.Token;
            Application.Current.Properties["ExpiryTime"] = deserializedResponse.ExpiryTime;
            Application.Current.Properties["Roles"] = deserializedResponse.Roles;

            await Application.Current.SavePropertiesAsync();

            return !deserializedResponse.LoginFailed;
        }

        public async Task<bool> ValidateLogin()
        {
            if (!Application.Current.Properties.ContainsKey("Token")) return false;

            RestUrl = App.ApiUrl + "Account/ValidateLogin";

            var messageBody = GetRequest(RestUrl, "");

            var response = await _client.SendAsync(messageBody);
         
            if (!response.IsSuccessStatusCode) return false;

            var content = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<LoginResponse>(content);

            return !deserializedResponse.LoginFailed;
        }

        public async Task<bool> Logout()
        {
            if (!Application.Current.Properties.ContainsKey("Token")) return false;
            
            RestUrl = App.ApiUrl + "Account/Logout";

            var messageBody = GetRequest(RestUrl, "");
            
            var response = await _client.SendAsync(messageBody);
         
            if (!response.IsSuccessStatusCode) return false;

            var content = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(content);

            Application.Current.Properties.Remove("Email");
            Application.Current.Properties.Remove("Token");
            Application.Current.Properties.Remove("ExpiryTime");
            await Application.Current.SavePropertiesAsync();

            return deserializedResponse;
        }
    }
}