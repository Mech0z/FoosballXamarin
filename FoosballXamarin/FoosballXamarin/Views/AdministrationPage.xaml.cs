using FoosballXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoosballXamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AdministrationPage : ContentPage
	{
	    private readonly AdministrationViewModel _viewModel;

	    public bool IsAdmin => _viewModel.IsAdmin;

		public AdministrationPage ()
		{
		    BindingContext = _viewModel = new AdministrationViewModel();
			InitializeComponent ();
		}
	}
}