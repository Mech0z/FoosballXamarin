using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FoosballXamarin.Helpers;
using FoosballXamarin.Models;
using FoosballXamarin.Services;
using Models;
using Xamarin.Forms;

namespace FoosballXamarin.ViewModels
{
	public class LastMatchesViewModel : BaseViewModel
	{
	    public IMatchService MatchService => DependencyService.Get<IMatchService>();
	    public IUserService UserService => DependencyService.Get<IUserService>();
	    public ObservableRangeCollection<Match> Matches{ get; set; }
	    public Command LoadItemsCommand { get; set; }

        public LastMatchesViewModel()
		{
			Title = "Last Games";

			//OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));

            Matches = new ObservableRangeCollection<Match>();
		    LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

		    MessagingCenter.Subscribe<LeaderBoardViewModel>(this, "SignalR-MatchAdded",
		        (sender) => Device.BeginInvokeOnMainThread(async () => { await ExecuteLoadItemsCommand(); }));
            //MessagingCenter.Subscribe<AddMatchViewModel>(this, "MatchAddedSuccessfully", async (sender) => await ExecuteLoadItemsCommand());

            LoadItemsCommand.Execute(this);
		}

        /// <summary>
        /// Command to open browser to xamarin.com
        /// </summary>
        //public ICommand OpenWebCommand { get; }

	    async Task ExecuteLoadItemsCommand()
	    {
	        IsBusy = true;

	        try
	        {
	            var matches = await MatchService.GetDataAsync();
	            var users = await UserService.GetDataAsync();

	            foreach (Match match in matches)
	            {
	                match.UserList = new List<User>
	                {
	                    users.Single(x => x.Email == match.PlayerList[0]),
	                    users.Single(x => x.Email == match.PlayerList[1]),
	                    users.Single(x => x.Email == match.PlayerList[2]),
	                    users.Single(x => x.Email == match.PlayerList[3]),
	                };
	            }

                Matches.ReplaceRange(matches);
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