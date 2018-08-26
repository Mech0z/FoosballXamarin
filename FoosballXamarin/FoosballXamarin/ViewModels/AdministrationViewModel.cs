using System;
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

        public ObservableRangeCollection<UserMapping> UserMappings { get; set; }
        public ObservableRangeCollection<Season> Seasons { get; set; }
        public Command LoadItemsCommand { get; set; }
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
            Seasons = new ObservableRangeCollection<Season>();
            MessagingCenter.Subscribe<LoginViewModel>(this, "LoginSuccessful",
                async (sender) =>
                {
                    await ExecuteLoadCommand();
                    await CheckRolesCommand();
                });
            MessagingCenter.Subscribe<LoginViewModel>(this, "LogoutSuccessful",
                async (sender) => await CheckRolesCommand());

            LoadItemsCommand = new Command(async () => await ExecuteLoadCommand());
        }

        public async Task StartNewSeason()
        {
            await AdministrationService.StartNewSeason();

            LoadItemsCommand.Execute(this);
        }

        private async Task CheckRolesCommand()
        {
            if (Preferences.ContainsKey("UserSettings"))
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

        async Task ExecuteLoadCommand()
        {
            IsBusy = true;
            try
            {
                var seasons = await AdministrationService.GetSeasons();
                Seasons.ReplaceRange(seasons.OrderByDescending(x => x.StartDate));
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
    }
}