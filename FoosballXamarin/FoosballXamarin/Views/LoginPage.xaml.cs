using System;
using System.Threading.Tasks;
using FoosballXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoosballXamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
    {
		public LoginPage ()
		{
			InitializeComponent ();

		    BindingContext = new LoginViewModel();

            MessagingCenter.Subscribe<LoginViewModel>(this, "Logout failed",
		        async (sender) => { await DisplayAlert("Error", "Error logging in!", "OK"); });
		}

        private void RequestPasswordButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RequestPasswordPage());
        }

	    private void CreateUserButton_OnClicked(object sender, EventArgs e)
	    {
	        Navigation.PushAsync(new CreateUserPage());
	    }
	}
}