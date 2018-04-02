using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoosballXamarin.Helpers;
using FoosballXamarin.Services;
using Models;
using Xamarin.Forms;

namespace FoosballXamarin.ViewModels
{
	public class ItemDetailViewModel : BaseViewModel
	{
	    public IUserService UserService => DependencyService.Get<IUserService>();
	    public IMatchService MatchService => DependencyService.Get<IMatchService>();

	    public Command LoadItemsCommand { get; set; }
        public LeaderboardViewEntry Item { get; set; }
	    public ObservableRangeCollection<Match> LatestMatches { get; set; }

        public ItemDetailViewModel(LeaderboardViewEntry item)
		{
			Title = item.UserName;
			Item = item;

            LatestMatches = new ObservableRangeCollection<Match>();

		    LoadItemsCommand = new Command(async () => await ExecuteLoadCommand());
            LoadItemsCommand.Execute(this);
        }

		private int _eloRating;
        public int EloRating
		{
			get => _eloRating;
		    set => SetProperty(ref _eloRating, value);
		}

        User _user;
	    public User User
	    {
	        get => _user;
	        set => SetProperty(ref _user, value);
	    }

        async Task ExecuteLoadCommand()
        {
            IsBusy = true;
            try
            {
                var users = await UserService.GetDataAsync();
                User = users.SingleOrDefault(x => x.Email == Item.UserName);

                LatestMatches.ReplaceRange(await MatchService.GetPlayerMatches(Item.UserName));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}