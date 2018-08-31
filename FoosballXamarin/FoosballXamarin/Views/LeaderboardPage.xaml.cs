using System;
using FoosballXamarin.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FoosballXamarin.ViewModels;
using Models;

namespace FoosballXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeaderBoardPage : ContentPage
    {
        readonly LeaderBoardViewModel _viewModel;

        public LeaderBoardPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new LeaderBoardViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (!(args.SelectedItem is LeaderboardViewEntry item))
                return;

            await Navigation.PushAsync(new PlayerDetailsPage(new PlayerDetailsViewModel(item)));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadItemsCommand.Execute(null);
        }
    }
}