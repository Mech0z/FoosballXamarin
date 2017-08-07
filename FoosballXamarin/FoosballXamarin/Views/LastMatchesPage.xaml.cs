
using FoosballXamarin.ViewModels;
using Xamarin.Forms;

namespace FoosballXamarin.Views
{
	public partial class LastMatchesPage : ContentPage
	{
	    readonly LastMatchesViewModel _viewModel;

        public LastMatchesPage()
		{
			InitializeComponent();

		    BindingContext = _viewModel = new LastMatchesViewModel();
        }
    }
}
