using System.Threading.Tasks;
using FoosballXamarin.Services;
using Xamarin.Forms;

namespace FoosballXamarin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ILoginService LoginService => DependencyService.Get<ILoginService>();

        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsLoggedIn { get; set; }

        public LoginViewModel()
        {
            Email = "madsskipper@gmail.com";
            Password = "Super123!";
        }

        public async Task<bool> LoginCommand()
        {
            //await LoginService.Login(Email, Password, true);
            await LoginService.ValidateLogin();

            return true;
        }
    }
}