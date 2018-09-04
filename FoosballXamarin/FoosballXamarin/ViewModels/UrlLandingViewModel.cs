using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FoosballXamarin.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoosballXamarin.ViewModels
{
    public class UrlLandingViewModel : BaseViewModel
    {
        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private string _apiEntry;
        public string ApiEntry
        {
            get => _apiEntry;
            set => SetProperty(ref _apiEntry, value);
        }

        private string _messageLabelText;
        public string MessageLabelText
        {
            get => _messageLabelText;
            set => SetProperty(ref _messageLabelText, value);
        }

        public ICommand SubmitApiUrlCommand { get; set; }
        public ICommand OpenGitHubLinkCommand { get; set; }
        public ILeaderboardService LeaderboardService => DependencyService.Get<ILeaderboardService>();

        public UrlLandingViewModel()
        {
            OpenGitHubLinkCommand = new Command(() => Device.OpenUri(new Uri("https://github.com/Mech0z/Foosball")));

            SubmitApiUrlCommand = new Command(async () =>
            {
                Preferences.Set("ApiUrlSettings", ApiEntry);
                IsBusy = true;
                var isValid = await CanPingLeaderboard();
                if (isValid)
                {
                    MessagingCenter.Send(this, "ApiPingSuccess");
                }
                else
                {
                    MessageLabelText = "Could not ping api successfully, remember http(s):// in the front and /api/ in the end";
                }
            });
        }

        private async Task<bool> CanPingLeaderboard()
        {
            try
            {
                var data = await LeaderboardService.GetDataAsync();
                IsBusy = false;
                return data != null;
            }
            catch (Exception e)
            {
                Preferences.Remove("ApiUrlSettings");
                IsBusy = false;
                return false;
            }
        }
    }
}