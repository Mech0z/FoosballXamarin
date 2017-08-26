using System;
using System.Collections.ObjectModel;
using System.Linq;
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
            if (IsBusy)
                return;

            var success = await _viewModel.SubmitMatch();
            if (success)
            {
                await DisplayAlert("Matches added", "Great success!", "OK");
                await Navigation.PopToRootAsync(true);
            }
        }

        private async Task PlayerClickedCommand(object sender, SelectedItemChangedEventArgs e)
        {
            var listview = sender as ListView;

            if(listview == null)
                return;

            var user = listview.SelectedItem as User;

            if (user == null)
                return;

            if (_viewModel.Team1.Contains(user))
            {
                var result = await DisplayActionSheet($"Swap {user.Username} with whom", "Cancel", null,
                    _viewModel.Team2[0].Username, _viewModel.Team2[1].Username);

                if (result == null || result == "Cancel")
                    return;

                var index = _viewModel.Team2.IndexOf(_viewModel.Team2.SingleOrDefault(x => x.Username == result));
                SwapPlayer(user, index);
                Team1ListView.SelectedItem = null;
            }
            else
            {
                var result = await DisplayActionSheet($"Swap {user.Username} with whom", "Cancel", null,
                    _viewModel.Team1[0].Username, _viewModel.Team1[1].Username);

                if (result == null || result == "Cancel")
                    return;

                var index = _viewModel.Team1.IndexOf(_viewModel.Team1.SingleOrDefault(x => x.Username == result));
                SwapPlayer(user, index);
                Team2ListView.SelectedItem = null;
            }
        }

        private void SwapPlayer(User user, int index)
        {
            if (_viewModel.Team1.Contains(user))
            {
                _viewModel.Team1.Remove(user);
                _viewModel.Team2.Add(user);

                _viewModel.Team1.Add(_viewModel.Team2[index]);
                _viewModel.Team2.Remove(_viewModel.Team2[index]);
            }
            else
            {
                _viewModel.Team2.Remove(user);
                _viewModel.Team1.Add(user);

                _viewModel.Team2.Add(_viewModel.Team1[index]);
                _viewModel.Team1.Remove(_viewModel.Team1[index]);
            }
        }
    }
}