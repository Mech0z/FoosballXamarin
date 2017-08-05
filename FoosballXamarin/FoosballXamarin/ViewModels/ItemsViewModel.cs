using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FoosballXamarin.Helpers;
using FoosballXamarin.Models;
using FoosballXamarin.Views;
using Xamarin.Forms;
using FoosballXamarin.Services;
using Models;

namespace FoosballXamarin.ViewModels
{
	public class ItemsViewModel : BaseViewModel
	{
	    private LeaderboardView _selectedLeaderboardView;
	    public IUserService UserService => DependencyService.Get<IUserService>();
	    public ILeaderboardService LeaderboardService => DependencyService.Get<ILeaderboardService>();
        public ObservableRangeCollection<Item> Items { get; set; }
		public ObservableRangeCollection<User> Users { get; set; }
		public ObservableRangeCollection<LeaderboardView> Leaderboards { get; set; }

	    public LeaderboardView SelectedLeaderboardView
	    {
	        get => _selectedLeaderboardView;
	        set => SetProperty(ref _selectedLeaderboardView, value);
	    }

	    public Command LoadItemsCommand { get; set; }

		public ItemsViewModel()
		{
			Title = "Browse";
			Items = new ObservableRangeCollection<Item>();
            Users = new ObservableRangeCollection<User>();
            Leaderboards = new ObservableRangeCollection<LeaderboardView>();
			LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

			MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
			{
				var _item = item as Item;
				Items.Add(_item);
				await DataStore.AddItemAsync(_item);
			});
		}

		async Task ExecuteLoadItemsCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
			    Users.ReplaceRange(await UserService.GetDataAsync());
                Leaderboards.ReplaceRange(await LeaderboardService.GetDataAsync());
                SelectedLeaderboardView = Leaderboards.OrderByDescending(x => x.Timestamp).FirstOrDefault();


                Items.Clear();
				var items = await DataStore.GetItemsAsync(true);
				Items.ReplaceRange(items);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				MessagingCenter.Send(new MessagingCenterAlert
				{
					Title = "Error",
					Message = "Unable to load items.",
					Cancel = "OK"
				}, "message");
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}