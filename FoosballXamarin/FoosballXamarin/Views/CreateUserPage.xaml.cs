using System;
using System.Threading.Tasks;
using FoosballXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoosballXamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateUserPage : ContentPage
	{
        public CreateUserPage ()
		{
			InitializeComponent ();
            
            MessagingCenter.Subscribe<CreateUserViewModel, string>(this, "CreateUserFailedMessage",
		        (sender, message) => { MessageLabel.Text = message; });
		    MessagingCenter.Subscribe<CreateUserViewModel>(this, "CreateUserSuccessMessage",
		        sender => { HandleSuccess(); });
		}

	    private async void HandleSuccess()
	    {
	        await DisplayAlert("Success", "User created", "OK");
	        await Navigation.PopAsync(true);
	    }
	}
}