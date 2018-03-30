using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoosballXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoosballXamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
	    private readonly LoginViewModel _viewModel;

		public LoginPage ()
		{
            BindingContext = _viewModel = new LoginViewModel();
			InitializeComponent ();
		}
        
	    private async Task LoginCommand(object sender, EventArgs e)
	    {
	        await _viewModel.LoginCommand();
	    }
	}
}