using System;
using System.Collections.Generic;
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

	    private bool _isLandscapeMode;
	    public bool IsLandscapeMode
	    {
	        get => _isLandscapeMode;
	        set => SetProperty(ref _isLandscapeMode, value);
	    }

	    public Command LoadItemsCommand { get; set; }

		public ItemsViewModel()
		{
			Title = "Browse";
            Users = new ObservableRangeCollection<User>();
            Leaderboards = new ObservableRangeCollection<LeaderboardView>();
			LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
		}

	    public void UpdateOrientationCommand(double height, double width)
	    {
	        IsLandscapeMode = height < width;
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

			            entry.FormList = GenerateFormList(entry.Form);
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

	    private List<Form> GenerateFormList(string entryForm)
	    {
	        var result = new List<Form>();
            
            foreach (char t in entryForm)
	        {
	            result.Add(t == 'W' ? new Form(1) : new Form(-1));
	        }

	        return result;
	    }
	}
}