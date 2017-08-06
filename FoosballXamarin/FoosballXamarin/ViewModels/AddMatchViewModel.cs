using System;
using System.Linq;
using System.Threading.Tasks;
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