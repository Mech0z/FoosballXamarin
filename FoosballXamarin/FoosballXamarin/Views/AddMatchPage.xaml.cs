using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FoosballXamarin.Helpers;
using FoosballXamarin.ViewModels;
using Models;
using Xamarin.Forms;

namespace FoosballXamarin.Views
{
    public partial class AddMatchPage : ContentPage
    {
        readonly AddMatchViewModel _viewModel;

        public AddMatchPage(ObservableRangeCollection<LeaderboardViewEntry> viewModelAddedPlayers)
        {
            InitializeComponent();

            BindingContext = _viewModel = new AddMatchViewModel(viewModelAddedPlayers);
        }

        private async Task SubmitCommand(object sender, EventArgs e)
        {
            var success = await _viewModel.SubmitMatch();
            if (success)
            {
                await Navigation.PushAsync(new ItemsPage());
            }
        }
    }
}