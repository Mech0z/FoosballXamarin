using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FoosballXamarin.Models.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FoosballXamarin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private bool _isInLoginMode;
        public bool IsInLoginMode
        {
            get => _isInLoginMode;
            set => SetProperty(ref _isInLoginMode, value);
        }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set => SetProperty(ref _isLoggedIn, value);
        }

        public LoginViewModel()
        {
            Email = "foostest@sharklasers.com";
            Password = "Super123!";
        }

        public async Task LoginCommand()
        {
            var accessToken = await LoginAsync(Email, Password);
            IsLoggedIn = true;
        }

        public async Task<string> LoginAsync(string userName, string password)
        {
            var restUrl = App.ApiUrl + "Account/Login";
            Uri httpUri = new Uri(string.Format(restUrl, string.Empty));
            var model = new LoginBindingModel
            {
                Email = Email,
                Password = Password,
                RememberMe = RememberMe
            };
            
            var json = JsonConvert.SerializeObject(model);

            var client = new HttpClient();

            var httpContent = new StringContent(json);

            var response = await client.PostAsync(httpUri, httpContent);

            var content = await response.Content.ReadAsStringAsync();
            
            JObject jwtDynamic = JsonConvert.DeserializeObject<dynamic>(content);
            return jwtDynamic.Value<string>("access_token");
        }
    }
}