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
            Email = "madsskipper@gmail.com";
            Password = "Super123!";
        }

        public async Task<bool> LoginCommand()
        {
            //await LoginService.Login(Email, Password, true);
            await LoginService.ValidateLogin();

            return true;
        }
    }
}