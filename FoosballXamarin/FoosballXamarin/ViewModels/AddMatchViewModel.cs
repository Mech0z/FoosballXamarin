using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foosball9000Api.RequestResponse;
using FoosballXamarin.Helpers;
using FoosballXamarin.Services;
using FoosballXamarin.Views;
using Models;
using Xamarin.Forms;

namespace FoosballXamarin.ViewModels
{
    public class AddMatchViewModel : BaseViewModel
    {
        private readonly ObservableRangeCollection<LeaderboardViewEntry> _viewModelAddedPlayers;
        public IUserService UserService => DependencyService.Get<IUserService>();
        public ILeaderboardService LeaderboardService => DependencyService.Get<ILeaderboardService>();
        public IMatchService MatchService => DependencyService.Get<IMatchService>();

        public ObservableRangeCollection<LeaderboardViewEntry> AddedPlayers { get; set; }
        public ObservableRangeCollection<User> Team1 { get; set; }
        public ObservableRangeCollection<User> Team2 { get; set; }
        public Command LoadCommand { get; set; }

        int _scoreTeam1Match1;
        public int ScoreTeam1Match1
        {
            get => _scoreTeam1Match1;
            set => SetProperty(ref _scoreTeam1Match1, value);
        }

        int _scoreTeam2Match1;
        public int ScoreTeam2Match1
        {
            get => _scoreTeam2Match1;
            set => SetProperty(ref _scoreTeam2Match1, value);
        }

        int _scoreTeam1Match2;
        public int ScoreTeam1Match2
        {
            get => _scoreTeam1Match2;
            set => SetProperty(ref _scoreTeam1Match2, value);
        }

        int _scoreTeam2Match2;
        public int ScoreTeam2Match2
        {
            get => _scoreTeam2Match2;
            set => SetProperty(ref _scoreTeam2Match2, value);
        }

        public AddMatchViewModel(ObservableRangeCollection<LeaderboardViewEntry> viewModelAddedPlayers)
        {
            Title = "Add Result";

            _viewModelAddedPlayers = viewModelAddedPlayers;
            Team1 = new ObservableRangeCollection<User>();
            Team2 = new ObservableRangeCollection<User>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            LoadCommand.Execute(this);
        }

        public async Task<bool> SubmitMatch()
        {
            if (IsBusy)
                return false;

            IsBusy = true;

            try
            {
                var users = new List<string>
                {
                    Team1[0].Email,
                    Team1[1].Email,
                    Team2[0].Email,
                    Team2[1].Email,
                };

                var request = new SaveMatchesRequest();
                request.Matches = new List<Match>
                {
                    new Match
                    {
                        PlayerList = users,
                        TimeStampUtc = DateTime.Now.AddMinutes(-5),
                        MatchResult = new MatchResult
                        {
                            Team1Score = ScoreTeam1Match1,
                            Team2Score = ScoreTeam2Match1
                        }
                    },
                    new Match
                    {
                        PlayerList = users,
                        TimeStampUtc = DateTime.Now,
                        MatchResult = new MatchResult
                        {
                            Team1Score = ScoreTeam1Match2,
                            Team2Score = ScoreTeam2Match2
                        }
                    }
                };

                var success = await MatchService.SubmitMatches(request);
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

            return false;
        }

        async Task ExecuteLoadCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var users = await UserService.GetDataAsync();

                Team1.Add(users.Single(x => x.Email == _viewModelAddedPlayers[0].UserName));
                Team1.Add(users.Single(x => x.Email == _viewModelAddedPlayers[3].UserName));
                Team2.Add(users.Single(x => x.Email == _viewModelAddedPlayers[1].UserName));
                Team2.Add(users.Single(x => x.Email == _viewModelAddedPlayers[2].UserName));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}