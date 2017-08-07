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

        public Command LoadCommand { get; set; }

        User _user1;
        public User User1
        {
            get => _user1;
            set => SetProperty(ref _user1, value);
        }

        User _user2;
        public User User2
        {
            get => _user2;
            set => SetProperty(ref _user2, value);
        }

        User _user3;
        public User User3
        {
            get => _user3;
            set => SetProperty(ref _user3, value);
        }

        User _user4;
        public User User4
        {
            get => _user4;
            set => SetProperty(ref _user4, value);
        }

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
            _viewModelAddedPlayers = viewModelAddedPlayers;
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
                    User1.Email,
                    User2.Email,
                    User3.Email,
                    User4.Email
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

                User1 = users.Single(x => x.Email == _viewModelAddedPlayers[0].UserName);
                User2 = users.Single(x => x.Email == _viewModelAddedPlayers[1].UserName);
                User3 = users.Single(x => x.Email == _viewModelAddedPlayers[2].UserName);
                User4 = users.Single(x => x.Email == _viewModelAddedPlayers[3].UserName);
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