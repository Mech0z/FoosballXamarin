using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FoosballXamarin.Models;
using FoosballXamarin.Services;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoosballXamarin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email;
        private bool _isLoggedIn;

        public LoginViewModel()
        {
            LoadItemsCommand = new Command(async () => await ExecuteLoadCommand());
            LoadItemsCommand.Execute(this);
            
            LoginCommand = new Command(async () =>
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
            });

            LogoutCommand = new Command(async () =>
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
            });
        }

        public ILoginService LoginService => DependencyService.Get<ILoginService>();
        public ICommand LoadItemsCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set => SetProperty(ref _isLoggedIn, value);
        }

        private async Task ExecuteLoadCommand()
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
            IsLoggedIn = await LoginService.ValidateLogin();
            if (IsLoggedIn || Preferences.ContainsKey("UserSettings"))
            {
                var serilizedUserSettings = Preferences.Get("UserSettings", "");
                var userSettings = JsonConvert.DeserializeObject<UserSettings>(serilizedUserSettings);
                Email = userSettings.Email;
                IsLoggedIn = true;
                MessagingCenter.Send(this, "LoginSuccessful");
            }
        }
    }
}