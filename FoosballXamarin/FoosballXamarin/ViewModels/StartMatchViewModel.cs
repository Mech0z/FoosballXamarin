using System;
using System.Linq;
using System.Threading.Tasks;
using FoosballXamarin.Helpers;
using FoosballXamarin.Services;
using FoosballXamarin.Views;
using Models;
using Xamarin.Forms;

namespace FoosballXamarin.ViewModels
{
    public class StartMatchViewModel : BaseViewModel
    {
        public IUserService UserService => DependencyService.Get<IUserService>();
        public ILeaderboardService LeaderboardService => DependencyService.Get<ILeaderboardService>();
        public ObservableRangeCollection<LeaderboardViewEntry> LeaderboardViewEntries { get; set; }
        public ObservableRangeCollection<User> Users { get; set; }
        public ObservableRangeCollection<LeaderboardViewEntry> AddedPlayers { get; set; }

        public Command LoadCommand { get; set; }
        
        public StartMatchViewModel()
        {
            Title = "Leaderboard";

            LeaderboardViewEntries = new ObservableRangeCollection<LeaderboardViewEntry>();
            Users = new ObservableRangeCollection<User>();
            AddedPlayers = new ObservableRangeCollection<LeaderboardViewEntry>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            LoadCommand.Execute(this);

            MessagingCenter.Subscribe<StartMatchPage, LeaderboardViewEntry>(this, "AddPlayerToGame",(obj, item) =>
            {
                if (AddedPlayers.Contains(item))
                {
                    AddedPlayers.Remove(item);
                    return;
                }

                if (AddedPlayers.Count >= 4)
                {
                    return;
                }

                AddedPlayers.Add(item);
                var ordered = AddedPlayers.OrderByDescending(x => x.EloRating).ToList();
                AddedPlayers.ReplaceRange(ordered);
            });

            MessagingCenter.Subscribe<StartMatchPage, LeaderboardViewEntry>(this, "AddedPlayerTapped", (obj, item) =>
            {
                AddedPlayers.Remove(item);
            });
        }

        async Task ExecuteLoadCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var leaderboardViews = await LeaderboardService.GetDataAsync();

                var newest = leaderboardViews.OrderByDescending(x => x.Timestamp).FirstOrDefault();

                var users = await UserService.GetDataAsync();

                LeaderboardViewEntries.ReplaceRange(newest.Entries);
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