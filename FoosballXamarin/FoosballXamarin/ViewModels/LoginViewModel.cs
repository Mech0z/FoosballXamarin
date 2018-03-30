﻿using System.Threading.Tasks;
using FoosballXamarin.Services;
using Xamarin.Forms;

namespace FoosballXamarin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ILoginService LoginService => DependencyService.Get<ILoginService>();
        
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
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

            //Todo do in background so we dont lock app bootup
            Device.BeginInvokeOnMainThread(() => { ValidateLogin(); });
        }

        public async Task ValidateLogin()
        {
            IsLoggedIn =  await LoginService.ValidateLogin();
            if (IsLoggedIn)
            {
                Email = Application.Current.Properties["Email"] as string;
                MessagingCenter.Send(this, "LoginSuccessful");
            }
        }

        public async Task<bool> LoginCommand()
        {
            var success = await LoginService.Login(Email, Password, true);

            if (success)
            {
                IsLoggedIn = true;
                MessagingCenter.Send(this, "LoginSuccessful");
            }

            return true;
        }

        public async Task Logout()
        {
            var success = await LoginService.Logout();
            if (success)
            {
                IsLoggedIn = false;
                MessagingCenter.Send(this, "LogoutSuccessful");
            }
        }
    }
}