using System;
using System.Linq;
using System.Threading.Tasks;
using FoosballXamarin.Helpers;
using FoosballXamarin.Models;
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

        bool _showAllPlayers;
        public bool ShowAllPlayers
        {
            get => _showAllPlayers;
            set
            {
                SetProperty(ref _showAllPlayers, value);
                ShowLeaderboard = !value;
            }
        }

        bool _showLeaderboard;
        public bool ShowLeaderboard
        {
            get => _showLeaderboard;
            set => SetProperty(ref _showLeaderboard, value);
        }

        public Command LoadCommand { get; set; }
        
        public StartMatchViewModel()
        {
            Title = "Leaderboard";

            LeaderboardViewEntries = new ObservableRangeCollection<LeaderboardViewEntry>();
            Users = new ObservableRangeCollection<User>();
            AddedPlayers = new ObservableRangeCollection<LeaderboardViewEntry>();
            ShowLeaderboard = true;

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

            MessagingCenter.Subscribe<StartMatchPage, User>(this, "AddUserToGame", (obj, item) =>
            {
                var alreadyAdded = AddedPlayers.SingleOrDefault(x => x.UserName == item.Email);

                if (alreadyAdded != null)
                {
                    AddedPlayers.Remove(alreadyAdded);
                    return;
                }
                
                if (AddedPlayers.Count >= 4)
                {
                    return;
                }

                var existingUserInCurrentLeaderboard =
                    LeaderboardViewEntries.SingleOrDefault(x => x.UserName == item.Email);

                if (existingUserInCurrentLeaderboard == null)
                {
                    AddedPlayers.Add(new LeaderboardViewEntry
                    {
                        Name = item.Username,
                        EloRating = 1500,
                        UserName = item.Email
                    });
                    var ordered = AddedPlayers.OrderByDescending(x => x.EloRating).ToList();
                    AddedPlayers.ReplaceRange(ordered);
                }
                else
                {
                    AddedPlayers.Add(existingUserInCurrentLeaderboard);
                    var ordered = AddedPlayers.OrderByDescending(x => x.EloRating).ToList();
                    AddedPlayers.ReplaceRange(ordered);
                }
            });

            MessagingCenter.Subscribe<StartMatchPage, LeaderboardViewEntry>(this, "AddedPlayerTapped", (obj, item) =>
            {
                AddedPlayers.Remove(item);
            });
        }

        public async Task Load()
        {
            if (!LeaderboardViewEntries.Any())
            {
                await InitialLoad();
                return;
            }

            IsBusy = true;

            try
            {
                var leaderboardViews = await LeaderboardService.GetDataAsync();

                var newest = leaderboardViews.OrderByDescending(x => x.Timestamp).FirstOrDefault();

                var users = await UserService.GetDataAsync();

                foreach (var entry in newest.Entries)
                {
                    entry.Name = users.SingleOrDefault(x => x.Email == entry.UserName).Username;
                }

                foreach (var entry in newest.Entries)
                {
                    var existingUser = LeaderboardViewEntries.SingleOrDefault(x => x.UserName == entry.UserName);

                    if (existingUser != null)
                    {
                        existingUser.EloRating = entry.EloRating;
                    }
                    else
                    {
                        LeaderboardViewEntries.Add(entry);
                    }
                }

                Users.ReplaceRange(users.OrderBy(x => x.Username));
                LeaderboardViewEntries.ReplaceRange(LeaderboardViewEntries.OrderByDescending(x => x.EloRating).ToList());
                AddedPlayers.ReplaceRange(AddedPlayers.OrderByDescending(x => x.EloRating).ToList());
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

        async Task InitialLoad()
        {
            IsBusy = true;

            try
            {
                var leaderboardViews = await LeaderboardService.GetDataAsync();

                var newest = leaderboardViews.OrderByDescending(x => x.Timestamp).FirstOrDefault();

                var users = await UserService.GetDataAsync();

                foreach (var entry in newest.Entries)
                {
                    entry.Name = users.SingleOrDefault(x => x.Email == entry.UserName).Username;
                }

                LeaderboardViewEntries.ReplaceRange(newest.Entries);
                Users.ReplaceRange(users.OrderBy(x => x.Username));
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