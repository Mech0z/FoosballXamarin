﻿using System;
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
            Users = new ObservableRangeCollection<User>();
            Leaderboards = new ObservableRangeCollection<LeaderboardView>();
			LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
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
                
                foreach (var leaderboard in Leaderboards)
			    {
			        foreach (var entry in leaderboard.Entries)
			        {
			            entry.Rank = leaderboard.Entries.IndexOf(entry) + 1;
			            entry.Name = Users.SingleOrDefault(x => x.Email == entry.UserName).Username;
                    }
			    }
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