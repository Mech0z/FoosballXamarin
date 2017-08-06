using System;
using FoosballXamarin.ViewModels;
using Models;
using Xamarin.Forms;

namespace FoosballXamarin.Views
{
	public partial class ItemsPage : ContentPage
	{
	    readonly ItemsViewModel _viewModel;

		public ItemsPage()
		{
			InitializeComponent();

			BindingContext = _viewModel = new ItemsViewModel();
		}

		async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			var item = args.SelectedItem as LeaderboardViewEntry;
			if (item == null)
				return;

			await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

			// Manually deselect item
			ItemsListView.SelectedItem = null;
		}

		void Refresh_Clicked(object sender, EventArgs e)
		{
		    _viewModel.LoadItemsCommand.Execute(this);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (_viewModel.Items.Count == 0)
				_viewModel.LoadItemsCommand.Execute(null);
		}
	}
}
