using System.Threading.Tasks;

namespace FoosballXamarin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginViewModel()
        {
            Email = "foostest@sharklasers.com";
            Password = "Super123!";
        }

        public Task LoginCommand()
        {
            return Task.Delay(1);
        }
    }
}