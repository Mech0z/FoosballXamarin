using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FoosballXamarin.Helpers;
using FoosballXamarin.Models;
using Xamarin.Forms;
using FoosballXamarin.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Models;

namespace FoosballXamarin.ViewModels
{
    public class LeaderBoardViewModel : BaseViewModel
    {
        private LeaderboardView _selectedLeaderboardView;
        public IPlayerService UserService => DependencyService.Get<IPlayerService>();
        public ILeaderboardService LeaderboardService => DependencyService.Get<ILeaderboardService>();
        public ObservableRangeCollection<User> Users { get; set; }
        public ObservableRangeCollection<LeaderboardView> Leaderboards { get; set; }
        private bool FirstLoad = true;

        public LeaderboardView SelectedLeaderboardView
        {
            get => _selectedLeaderboardView;
            set => SetProperty(ref _selectedLeaderboardView, value);
        }

        public ICommand LoadItemsCommand { get; set; }

        public LeaderBoardViewModel()
        {
            Title = "Leaderboard";
            Users = new ObservableRangeCollection<User>();
            Leaderboards = new ObservableRangeCollection<LeaderboardView>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            
            MessagingCenter.Subscribe<LeaderBoardViewModel>(this, "SignalR-MatchAdded",
                (sender) => Device.BeginInvokeOnMainThread(async () => { await ExecuteLoadItemsCommand(); }));
            

            LoadItemsCommand.Execute(this);
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (FirstLoad)
            {
                FirstLoad = false;
            }

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                List<LeaderboardView> leaderboardViews = await LeaderboardService.GetDataAsync();
                Users.ReplaceRange(await UserService.GetDataAsync());
                Leaderboards.ReplaceRange(leaderboardViews);
                SelectedLeaderboardView = Leaderboards.OrderByDescending(x => x.StartDate).FirstOrDefault();

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