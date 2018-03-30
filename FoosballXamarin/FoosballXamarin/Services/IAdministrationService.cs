using System.Collections.Generic;
using System.Threading.Tasks;
using FoosballXamarin.Models;

namespace FoosballXamarin.Services
{
    public interface IAdministrationService
    {
        Task<List<UserMapping>> GetUsermappings();
        Task<bool> ChangeUserPassword(string userEmail, string newPassword);
        Task<bool> ChangeUserRoles(string userEmail, List<string> newUserRoles);
    }
}