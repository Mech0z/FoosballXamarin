using FoosballXamarin.Helpers;
using FoosballXamarin.Models;
using FoosballXamarin.Services;

using Xamarin.Forms;

namespace FoosballXamarin.ViewModels
{
	public class BaseViewModel : ObservableObject
	{
		/// <summary>
		/// Get the azure service instance
		/// </summary>
		public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

		bool _isBusy = false;
		public bool IsBusy
		{
			get => _isBusy;
		    set => SetProperty(ref _isBusy, value);
		}
		/// <summary>
		/// Private backing field to hold the title
		/// </summary>
		string _title = string.Empty;
		/// <summary>
		/// Public property to set and get the title of the item
		/// </summary>
		public string Title
		{
			get => _title;
		    set => SetProperty(ref _title, value);
		}
	}
}

