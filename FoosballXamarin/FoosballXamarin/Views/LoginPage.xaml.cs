using System;
using System.Threading.Tasks;
using FoosballXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoosballXamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage
	{
	    private readonly LoginViewModel _viewModel;

		public LoginPage ()
		{
            BindingContext = _viewModel = new LoginViewModel();
			InitializeComponent ();

		    MessagingCenter.Subscribe<LoginViewModel>(this, "Logout failed",
		        async (sender) => { await DisplayAlert("Error", "Error logging in!", "OK"); });
		}
        
	    private async Task LoginCommand(object sender, EventArgs e)
	    {
            await _viewModel.LoginCommand();
	    }

	    private async Task LogoutCommand(object sender, EventArgs e)
	    {
	        await _viewModel.Logout();
	    }
	}
}