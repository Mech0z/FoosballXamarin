using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoosballXamarin.Services;
using FoosballXamarin.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoosballXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UrlLandingPage : ContentPage
    {
        //private readonly UrlLandingPageViewModel _viewModel;
        public ILeaderboardService LeaderboardService => DependencyService.Get<ILeaderboardService>();

        public UrlLandingPage()
        {
            InitializeComponent();
            //BindingContext = _viewModel = new UrlLandingPageViewModel();

        }

        private async Task Button_OnClicked(object sender, EventArgs e)
        {
            var apiUrl = UrlEntry.Text;

            Preferences.Set("ApiUrlSettings", apiUrl);
            var isValid = await CanPingLeaderboard(apiUrl);
            if (isValid)
            {
                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();
            }
            else
            {
                messageLabel.Text = "Login failed";
            }
        }

        private async Task<bool> CanPingLeaderboard(string apiUrl)
        {
            try
            {
                var data = await LeaderboardService.GetDataAsync();
                return data != null;
            }
            catch (Exception)
            {
                Preferences.Remove("ApiUrlSettings");
                return false;
            }
        }
    }
}