using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoosballXamarin.Helpers;
using FoosballXamarin.Models;
using FoosballXamarin.Services;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoosballXamarin.ViewModels
{
    public class AdministrationViewModel : BaseViewModel
    {
        public IAdministrationService AdministrationService => DependencyService.Get<IAdministrationService>();
        public IUserService UserService => DependencyService.Get<IUserService>();

        public ObservableRangeCollection<UserMapping> UserMappings{ get; set; }
        bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set => SetProperty(ref _isAdmin, value);
        }

        private UserMapping _selectedUser;
        public UserMapping SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }

        public async Task ItemsSelectedCommand(object sender)
        {
            if (sender is UserMapping selectedUser)
            {
                SelectedUser = selectedUser;
            }
        }

        public AdministrationViewModel()
        {
            UserMappings = new ObservableRangeCollection<UserMapping>();
            MessagingCenter.Subscribe<LoginViewModel>(this, "LoginSuccessful", async (sender) => await CheckRolesCommand());
            MessagingCenter.Subscribe<LoginViewModel>(this, "LogoutSuccessful", async (sender) => await CheckRolesCommand());
        }

        private async Task CheckRolesCommand()
        {
            if(Preferences.ContainsKey("UserSettings"))
            {
                var serilizedUserSettings = Preferences.Get("UserSettings", "");
                var userSettings = JsonConvert.DeserializeObject<UserSettings>(serilizedUserSettings);

                if (userSettings.Roles.Contains("Admin"))
                {
                    var result = await AdministrationService.GetUsermappings();
                    var users = await UserService.GetDataAsync();

                    foreach (UserMapping mapping in result)
                    {
                        mapping.DisplayName = users.SingleOrDefault(x => x.Email == mapping.Email)?.Username;
                    }

                    UserMappings.AddRange(result.OrderBy(x => x.DisplayName));
                    IsAdmin = true;
                    return;
                }
            }
            
            SelectedUser = null;
            IsAdmin = false;
        }
    }
}