using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FoosballXamarin.ViewModels;
using Models;
using Xamarin.Forms;

namespace FoosballXamarin.Views
{
    public partial class StartMatchPage : ContentPage
    {
        readonly StartMatchViewModel _viewModel;

        public StartMatchPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new StartMatchViewModel();
        }

        private async Task StartMatchCommand(object sender, EventArgs e)
        {
            if (_viewModel.AddedPlayers.Count != 4)
            {
                await DisplayAlert("Error", "Must select 4 players", "OK");
            }
            else
            {
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