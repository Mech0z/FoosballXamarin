using System.Threading.Tasks;
using System.Windows.Input;
using FoosballXamarin.Services;
using Xamarin.Forms;

namespace FoosballXamarin.ViewModels
{
    public class CreateUserViewModel :BaseViewModel
    {
        public IUserService UserService => DependencyService.Get<IUserService>();

        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string EmailConfirmation { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        private string _errorMessageText;
        public string ErrorMessageText
        {
            get => _errorMessageText;
            set => SetProperty(ref _errorMessageText, value);
        }

        public ICommand CreateUserCommand { get; set; }

        public CreateUserViewModel()
        {
            CreateUserCommand = new Command(async () => await ExecuteCreateUserCommand());
        }

        private async Task ExecuteCreateUserCommand()
        {
            if (DisplayName.Length < 6)
            {
                ErrorMessageText = "Username must be at least 6 characters long";
                return;
            }

            if (Email != EmailConfirmation)
            {
                ErrorMessageText = "Emails are not identical";
                return;
            }

            if (string.IsNullOrEmpty(Email) || !Email.Contains("@"))
            {
                ErrorMessageText = "Invalid email";
                return;
            }

            if (Password != PasswordConfirmation)
            {
                ErrorMessageText = "Passwords are not identical";
                return;
            }

            if (Password.Length < 6)
            {
                ErrorMessageText = "Password must be at least 6 characters long";
                return;
            }

            var result = await UserService.CreateUser(Email, DisplayName, Password);

            if (result)
                MessagingCenter.Send(this, "CreateUserSuccessMessage");
                    else
                ErrorMessageText = "User creation failed ;(";
        }
    }
}
