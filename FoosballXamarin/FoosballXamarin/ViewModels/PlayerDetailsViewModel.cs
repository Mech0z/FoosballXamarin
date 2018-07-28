using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoosballXamarin.Helpers;
using FoosballXamarin.Models;
using FoosballXamarin.Models.Dtos;
using FoosballXamarin.Services;
using Models;
using Xamarin.Forms;

namespace FoosballXamarin.ViewModels
{
	public class PlayerDetailsViewModel : BaseViewModel
	{
	    public IUserService UserService => DependencyService.Get<IUserService>();
	    public IMatchService MatchService => DependencyService.Get<IMatchService>();

	    public Command LoadItemsCommand { get; set; }
        public LeaderboardViewEntry Item { get; set; }
	    public ObservableRangeCollection<Match> LatestMatches { get; set; }

	    public ObservableRangeCollection<Match> FilteredMatches
	    {
	        get
	        {
	            var success = int.TryParse(SelectedLimit, out var parsedValue);

                if(LatestMatches.Count == 0)
                    return new ObservableRangeCollection<Match>();

	            return success
	                ? new ObservableRangeCollection<Match>(LatestMatches.OrderByDescending(x => x.TimeStampUtc)
	                    .Take(parsedValue))
	                : LatestMatches;
	        }
	    }

	    public PlayerDetailsViewModel(LeaderboardViewEntry item)
		{
			Title = item.UserName;
			Item = item;

            LatestMatches = new ObservableRangeCollection<Match>();
            PlayerLeaderBoardHistory = new ObservableRangeCollection<PlayerLeaderboardEntry>();

		    LoadItemsCommand = new Command(async () => await ExecuteLoadCommand());
            LoadItemsCommand.Execute(this);
		    SelectedLimit = LimitRanges.FirstOrDefault();
		}

        public List<string> LimitRanges => new List<string>{"10", "25", "100", "All"};

	    private string _selectedLimit;
	    public string SelectedLimit
	    {
	        get => _selectedLimit;
	        set
	        {
	            SetProperty(ref _selectedLimit, value);
	            OnPropertyChanged(nameof(FilteredMatches));
            }
	    }

	    private int _eloRating;
        public int EloRating
		{
			get => _eloRating;
		    set => SetProperty(ref _eloRating, value);
		}

	    private DateTime _fromDate;
	    public DateTime FromDate
	    {
	        get => _fromDate;
	        set
	        {
	            SetProperty(ref _fromDate, value);
	            OnPropertyChanged(nameof(FilteredMatches));
            }
	    }

	    private DateTime _toDate;
	    public DateTime ToDate
	    {
	        get => _toDate;
	        set
	        {
	            SetProperty(ref _toDate, value);
	            OnPropertyChanged(nameof(FilteredMatches));
	        }
	    }

	    private ObservableRangeCollection<PlayerLeaderboardEntry> _playerLeaderBoardHistory;
	    public ObservableRangeCollection<PlayerLeaderboardEntry> PlayerLeaderBoardHistory
        {
	        get => _playerLeaderBoardHistory;
	        set
	        {
	            SetProperty(ref _playerLeaderBoardHistory, value);
	            OnPropertyChanged(nameof(PlayerLeaderBoardHistory));
	        }
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

                var historyData = await UserService.GetPlayerSeasonHistory(User.Email);
                PlayerLeaderBoardHistory.ReplaceRange(historyData.PlayerLeaderboardEntries.OrderByDescending(x => x.SeasonName));
                LatestMatches.ReplaceRange(await MatchService.GetPlayerMatches(Item.UserName));
                OnPropertyChanged(nameof(FilteredMatches));
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