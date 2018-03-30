using System.Threading.Tasks;
using FoosballXamarin.Models;
using FoosballXamarin.ViewModels;
using Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoosballXamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AdministrationPage : ContentPage
	{
	    private readonly AdministrationViewModel _viewModel;

		public AdministrationPage ()
		{
		    BindingContext = _viewModel = new AdministrationViewModel();
			InitializeComponent ();
		}

	    private async Task ItemsSelectedCommand(object sender, SelectedItemChangedEventArgs e)
	    {
	        if (!(e.SelectedItem is UserMapping user))
	            return;

	        await _viewModel.ItemsSelectedCommand(user);
	    }
	}
}