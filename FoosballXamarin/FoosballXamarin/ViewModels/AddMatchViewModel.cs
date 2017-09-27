using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foosball9000Api.RequestResponse;
using FoosballXamarin.Helpers;
using FoosballXamarin.Services;
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

        Match _match1;
        public Match Match1
        {
            get => _match1;
            set => SetProperty(ref _match1, value);
        }

        Match _match2;
        public Match Match2
        {
            get => _match2;
            set => SetProperty(ref _match2, value);
        }

        public string ErrorMessage { get; set; }
        public Command LoadCommand { get; set; }
        
        bool _isNotSubmitting;
        public bool IsNotSubmitting
        {
            get => _isNotSubmitting;
            set => SetProperty(ref _isNotSubmitting, value);
        }

        public AddMatchViewModel(ObservableRangeCollection<LeaderboardViewEntry> viewModelAddedPlayers)
        {
            Title = "Add Result";

            _viewModelAddedPlayers = viewModelAddedPlayers;
            Team1 = new ObservableRangeCollection<User>();
            Team2 = new ObservableRangeCollection<User>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            LoadCommand.Execute(this);
            IsNotSubmitting = true;
            Match1 = new Match();
            Match2 = new Match();
        }

        public async Task<bool> SubmitMatch()
        {
            if (IsBusy)
                return false;

            IsBusy = true;
            IsNotSubmitting = false;

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


                Match1.PlayerList = users;
                Match1.TimeStampUtc = DateTime.Now.AddMinutes(-5);

                Match2.PlayerList = users;
                Match2.TimeStampUtc = DateTime.Now;
                
                request.Matches = new List<Match>();

                if (!Match1.HaveScore && !Match2.HaveScore)
                {
                    ErrorMessage = "At least add one score";
                    return false;
                }

                if (Match1.HaveScore)
                {
                    if (!Match1.IsValid)
                    {
                        ErrorMessage = Match1.MatchValidationErrorText;
                        return false;
                    }
                    request.Matches.Add(Match1);
                }
                if (Match2.HaveScore)
                {
                    if (!Match2.IsValid)
                    {
                        ErrorMessage = Match2.MatchValidationErrorText;
                        return false;
                    }
                    request.Matches.Add(Match2);
                }

                var success = await MatchService.SubmitMatches(request);
                MessagingCenter.Send(this, "MatchAddedSuccessfully");
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
                IsNotSubmitting = true;
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