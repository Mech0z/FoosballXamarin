using System;
using System.Threading.Tasks;
using FoosballXamarin.Models;
using FoosballXamarin.ViewModels;
using Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;

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

	    private void AddPlayerRoleClicked(object sender, EventArgs e)
	    {

	    }

	    private async Task ChangeUserPassWordClicked(object sender, EventArgs e)
	    {
	        var newPasswordPromptResult = await UserDialogs.Instance.PromptAsync("", "New Password", "Save", null);

	        if (newPasswordPromptResult.Ok)
	        {
	            bool toSave = await UserDialogs.Instance.ConfirmAsync(
	                $"Password will be changed to {newPasswordPromptResult.Text} for {_viewModel.SelectedUser.DisplayName}");
	            if (toSave)
	            {
	                var changeResult = await
	                    _viewModel.AdministrationService.ChangeUserPassword(_viewModel.SelectedUser.Email,
	                        newPasswordPromptResult.Text);
	                if (changeResult)
	                {
	                    await UserDialogs.Instance.AlertAsync("Password changed");
	                }
	                else
	                {
	                    await UserDialogs.Instance.AlertAsync("Password change failed!");
	                }
	            }
	        }
	    }
	}
}