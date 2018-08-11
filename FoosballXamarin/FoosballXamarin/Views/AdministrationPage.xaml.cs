using System;
using System.Linq;
using System.Threading.Tasks;
using FoosballXamarin.Models;
using FoosballXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;

namespace FoosballXamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AdministrationPage
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

	    private async Task AddPlayerRoleClicked(object sender, EventArgs e)
	    {
	        var newRole = await UserDialogs.Instance.PromptAsync("", "New Role", "Save", null);

	        if (newRole.Ok)
	        {
	            bool toSave = await UserDialogs.Instance.ConfirmAsync(
	                $"User {_viewModel.SelectedUser.DisplayName} will get role {newRole.Text}");
	            if (toSave)
	            {
                    _viewModel.SelectedUser.Roles.Add(newRole.Text);
	                var result = await _viewModel.AdministrationService.ChangeUserRoles(_viewModel.SelectedUser.Email,
	                    _viewModel.SelectedUser.Roles.ToList());

	                if (result)
	                {
	                    await UserDialogs.Instance.AlertAsync("Roles updated");
	                }
	                else
	                {
	                    await UserDialogs.Instance.AlertAsync("Roles updating failed!");
	                }
	            }}
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

	    private async Task RemovePlayerRoleClicked(object sender, EventArgs e)
	    {
            var roles = _viewModel.SelectedUser.Roles;
            var result = await UserDialogs.Instance.ActionSheetAsync("Remove role", "Cancel", "", null, roles.ToArray());
	        
	        _viewModel.SelectedUser.Roles.Remove(result);

	        if (result == "Cancel") return;

	        bool toSave = await UserDialogs.Instance.ConfirmAsync(
	            $"User {_viewModel.SelectedUser.DisplayName} will loose role {result}");
	        if (toSave)
	        {
	            var updatedList = _viewModel.SelectedUser.Roles.ToList();
	            updatedList.Remove(result);
	            var changeResult = await _viewModel.AdministrationService.ChangeUserRoles(_viewModel.SelectedUser.Email,
	                updatedList);

	            if (changeResult)
	            {
	                await UserDialogs.Instance.AlertAsync("Roles updated");
	                _viewModel.SelectedUser.Roles.Remove(result);
	            }
	            else
	            {
	                await UserDialogs.Instance.AlertAsync("Roles updating failed!");
	            }
	        }
	    }
	}
}