using System;
using System.Linq;
using Acr.UserDialogs;
using FoosballXamarin.Models;
using FoosballXamarin.ViewModels;
using Models;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoosballXamarin.Views
{
    public partial class StartMatchPage
    {
        readonly StartMatchViewModel _viewModel;

        public StartMatchPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new StartMatchViewModel();
        }

        private async void StartMatchCommand(object sender, EventArgs e)
        {
            if (_viewModel.AddedPlayers.Count != 4)
            {
                await UserDialogs.Instance.AlertAsync("Must select 4 players!");
            }
            else
            {
                if (!Preferences.ContainsKey("UserSettings") )
                {
                    await UserDialogs.Instance.AlertAsync("Login required!");
                    return;
                }

                var serilizedUserSettings = Preferences.Get("UserSettings", "");
                var userSettings = JsonConvert.DeserializeObject<UserSettings>(serilizedUserSettings);
                var emails = _viewModel.AddedPlayers.Select(player => player.UserName);
                if (!emails.Contains(userSettings.Email) && !userSettings.Roles.Contains("Admin"))
                {
                    await UserDialogs.Instance.AlertAsync("Must participate or be admin to submit match!");
                    return;
                }

                await Navigation.PushAsync(new AddMatchPage(_viewModel.AddedPlayers));
            }
        }

        protected override async void OnAppearing()
        {
            await _viewModel.Load();
            base.OnAppearing();
        }

        void Handle_AddedPlayerTapped(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var castedEntry = e.SelectedItem as LeaderboardViewEntry;

            MessagingCenter.Send(this, "AddedPlayerTapped", castedEntry);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        void Handle_ItemTapped(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var castedEntry = e.SelectedItem as LeaderboardViewEntry;

            MessagingCenter.Send(this, "AddPlayerToGame", castedEntry);
            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private void Handle_UserTapped(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var user = e.SelectedItem as User;

            MessagingCenter.Send(this, "AddUserToGame", user);
            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}