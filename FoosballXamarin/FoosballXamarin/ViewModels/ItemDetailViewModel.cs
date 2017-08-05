using FoosballXamarin.Models;
using Models;

namespace FoosballXamarin.ViewModels
{
	public class ItemDetailViewModel : BaseViewModel
	{
		public LeaderboardViewEntry Item { get; set; }
		public ItemDetailViewModel(LeaderboardViewEntry item = null)
		{
			Title = item.UserName;
			Item = item;
		}

		int eloRating = 1;
		public int EloRating
		{
			get { return eloRating; }
			set { SetProperty(ref eloRating, value); }
		}
	}
}