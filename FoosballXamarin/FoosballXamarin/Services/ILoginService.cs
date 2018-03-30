using System.Threading.Tasks;

namespace FoosballXamarin.Services
{
    public interface ILoginService
    {
        Task<bool> Login(string email, string password, bool rememberMe);
        Task<bool> ValidateLogin();
        Task<bool> Logout();
    }
}