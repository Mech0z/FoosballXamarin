using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using FoosballXamarin.Helpers;
using FoosballXamarin.Services;
using Models;
using Xamarin.Forms;

namespace FoosballXamarin.ViewModels
{
	public class LastMatchesViewModel : BaseViewModel
	{
	    public IMatchService MatchService => DependencyService.Get<IMatchService>();
	    public ObservableRangeCollection<Match> Matches{ get; set; }
	    public Command LoadItemsCommand { get; set; }

        public LastMatchesViewModel()
		{
			Title = "Last Games";

			//OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));

            Matches = new ObservableRangeCollection<Match>();
		    LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            LoadItemsCommand.Execute(this);
        }

        /// <summary>
        /// Command to open browser to xamarin.com
        /// </summary>
        //public ICommand OpenWebCommand { get; }

	    async Task ExecuteLoadItemsCommand()
	    {
	        if (IsBusy)
	            return;

	        IsBusy = true;

	        try
	        {
	            Matches.ReplaceRange(await MatchService.GetDataAsync());
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