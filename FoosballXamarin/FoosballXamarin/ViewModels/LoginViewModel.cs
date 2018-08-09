using System;
using System.Threading.Tasks;
using FoosballXamarin.Models;
using FoosballXamarin.Services;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoosballXamarin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ILoginService LoginService => DependencyService.Get<ILoginService>();
        public Command LoadItemsCommand { get; set; }
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

            LoadItemsCommand = new Command(async () => await ExecuteLoadCommand());
            LoadItemsCommand.Execute(this);
        }

        async Task ExecuteLoadCommand()
        {
            IsBusy = true;
            try
            {
                await ValidateLogin();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ValidateLogin()
        {
            IsLoggedIn =  await LoginService.ValidateLogin();
            if (IsLoggedIn || Preferences.ContainsKey("UserSettings"))
            {
                var serilizedUserSettings = Preferences.Get("UserSettings", "");
                var userSettings = JsonConvert.DeserializeObject<UserSettings>(serilizedUserSettings);
                Email = userSettings.Email;
                IsLoggedIn = true;
                MessagingCenter.Send(this, "LoginSuccessful");
            }
        }

        public async Task LoginCommand()
        {
            IsBusy = true;
            try
            {
                var success = await LoginService.Login(Email, Password, true);
                if (success)
                {
                    IsLoggedIn = true;
                    MessagingCenter.Send(this, "LoginSuccessful");
                }
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, "Login failed");
                Console.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task Logout()
        {
            IsBusy = true;
            try
            {
                var success = await LoginService.Logout();
                if (success)
                {
                    IsLoggedIn = false;
                    MessagingCenter.Send(this, "LogoutSuccessful");
                }
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, "Logout failed");
                Console.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}