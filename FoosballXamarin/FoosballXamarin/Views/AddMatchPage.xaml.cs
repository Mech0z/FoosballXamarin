using System.Collections.ObjectModel;
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
    }
}