using System.Collections.ObjectModel;
using FoosballXamarin.ViewModels;
using Models;
using Xamarin.Forms;

namespace FoosballXamarin.Views
{
    public partial class AddMatchPage : ContentPage
    {
        readonly AddMatchViewModel _viewModel;

        public AddMatchPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new AddMatchViewModel();
        }

        void Handle_AddedPlayerTapped(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var castedEntry = e.SelectedItem as LeaderboardViewEntry;

            MessagingCenter.Send(this, "AddedPlayerTapped", castedEntry);
            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

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
    }
}