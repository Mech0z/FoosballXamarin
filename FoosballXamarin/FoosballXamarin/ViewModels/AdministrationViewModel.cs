using System;
using System.Threading.Tasks;
using FoosballXamarin.Helpers;
using FoosballXamarin.Models;
using FoosballXamarin.Services;
using Xamarin.Forms;

namespace FoosballXamarin.ViewModels
{
    public class AdministrationViewModel : BaseViewModel
    {
        public IAdministrationService AdministrationService => DependencyService.Get<IAdministrationService>();

        public ObservableRangeCollection<UserMapping> UserMappings{ get; set; }
        bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set => SetProperty(ref _isAdmin, value);
        }

        public AdministrationViewModel()
        {
            UserMappings = new ObservableRangeCollection<UserMapping>();
            MessagingCenter.Subscribe<LoginViewModel>(this, "LoginSuccessful", async (sender) => await CheckRolesCommand());
            MessagingCenter.Subscribe<LoginViewModel>(this, "LogoutSuccessful", async (sender) => await CheckRolesCommand());
        }

        private async Task CheckRolesCommand()
        {
            if(Application.Current.Properties.ContainsKey("Email"))
            {
                var result = await AdministrationService.GetUsermappings();
                UserMappings.AddRange(result);
                IsAdmin = true;
            }
            else
            {
                IsAdmin = false;
            }
        }
    }
}