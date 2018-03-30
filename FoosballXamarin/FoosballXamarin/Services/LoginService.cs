using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FoosballXamarin.Models;
using FoosballXamarin.Services;
using Newtonsoft.Json;
using Plugin.DeviceInfo;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoginService))]
namespace FoosballXamarin.Services
{
    public class LoginService : BaseService, ILoginService
    {
        public async Task<bool> Login(string email, string password, bool rememberMe)
        {
            var deviceName = CrossDeviceInfo.Current.DeviceName;
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

            await Application.Current.SavePropertiesAsync();

            return !deserializedResponse.LoginFailed;
        }

        public async Task<bool> ValidateLogin()
        {
            if (!Application.Current.Properties.ContainsKey("Token")) return false;
            
            var token = Application.Current.Properties["Token"] as string;
            var email = Application.Current.Properties["Email"] as string;

            var deviceName = CrossDeviceInfo.Current.DeviceName;
            var request = new ValidateLoginRequest
            {
                Email = email,
                Token = token,
                DeviceName = deviceName
            };

            RestUrl = App.ApiUrl + "Account/ValidateLogin";
            
            var jsonRequest = JsonConvert.SerializeObject(request);
            var contentType = "application/json";

            var response = await _client.PostAsync(HttpUri, new StringContent(jsonRequest, Encoding.UTF8, contentType));
         
            if (!response.IsSuccessStatusCode) return false;

            var content = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<LoginResponse>(content);

            return !deserializedResponse.LoginFailed;
        }
    }

    public interface ILoginService
    {
        Task<bool> Login(string email, string password, bool rememberMe);
        Task<bool> ValidateLogin();
    }
}
