using System;
using System.Threading.Tasks;
using FoosballXamarin.Services;
using FoosballXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoosballXamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
	    private bool timeoutMessageShown;

		public LoginPage ()
		{
			InitializeComponent ();

		    BindingContext = new LoginViewModel();

            MessagingCenter.Subscribe<LoginViewModel>(this, "Logout failed",
		        async (sender) => { await DisplayAlert("Error", "Error logging in!", "OK"); });

		    MessagingCenter.Subscribe<HttpClientWrapper>(this, "TokenExpired",
		        async (sender) =>
		        {
		            if (!timeoutMessageShown)
		            {
		                timeoutMessageShown = true;
		                await DisplayAlert("Error", "Your login has expired, please login again", "OK");
                    }
		        });
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