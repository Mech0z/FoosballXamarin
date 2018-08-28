using System;
using System.Threading.Tasks;
using FoosballXamarin.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoosballXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UrlLandingPage
    {
        //private readonly UrlLandingPageViewModel _viewModel;
        public ILeaderboardService LeaderboardService => DependencyService.Get<ILeaderboardService>();

        public UrlLandingPage()
        {
            InitializeComponent();
        }

        private async Task Button_OnClicked(object sender, EventArgs e)
        {
            var apiUrl = UrlEntry.Text;

            Preferences.Set("ApiUrlSettings", apiUrl);
            var isValid = await CanPingLeaderboard();
            if (isValid)
            {
                ((App) Application.Current).MainPage = new MainPage();
            }
            else
            {
                MessageLabel.Text = "Login failed";
            }
        }

        private async Task<bool> CanPingLeaderboard()
        {
            try
            {
                var data = await LeaderboardService.GetDataAsync();
                return data != null;
            }
            catch (Exception e)
            {
                Preferences.Remove("ApiUrlSettings");
                return false;
            }
        }

        private void Button_GitHubLinkClicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://github.com/Mech0z/Foosball"));
        }
    }
}