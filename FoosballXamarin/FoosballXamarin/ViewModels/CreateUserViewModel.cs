using System.Threading.Tasks;
using System.Windows.Input;
using FoosballXamarin.Services;
using Xamarin.Forms;

namespace FoosballXamarin.ViewModels
{
    public class CreateUserViewModel
    {
        public IUserService UserService => DependencyService.Get<IUserService>();

        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string EmailConfirmation { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }

        public ICommand CreateUserCommand { get; set; }

        public CreateUserViewModel()
        {
            CreateUserCommand = new Command(async () => await ExecuteCreateUserCommand());
        }

        private async Task ExecuteCreateUserCommand()
        {
            if (DisplayName.Length < 6)
            {
                MessagingCenter.Send(this, "CreateUserFailedMessage", "Username must be at least 6 characters long");
                return;
            }

            if (Email != EmailConfirmation)
            {
                MessagingCenter.Send(this, "CreateUserFailedMessage", "Emails are not identical");
                return;
            }

            if (string.IsNullOrEmpty(Email) || !Email.Contains("@"))
            {
                MessagingCenter.Send(this, "CreateUserFailedMessage", "Invalid email");
                return;
            }

            if (Password != PasswordConfirmation)
            {
                MessagingCenter.Send(this, "CreateUserFailedMessage", "Passwords are not identical");
                return;
            }

            if (Password.Length < 6)
            {
                MessagingCenter.Send(this, "CreateUserFailedMessage", "Password must be at least 6 characters long");
                return;
            }

            var result = await UserService.CreateUser(Email, DisplayName, Password);

            if (result)
                MessagingCenter.Send(this, "CreateUserSuccessMessage");
                    else
                MessagingCenter.Send(this, "CreateUserFailedMessage", "User creation failed ;(");
        }
    }
}
