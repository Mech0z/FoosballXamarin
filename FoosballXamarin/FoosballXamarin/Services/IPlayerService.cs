using System.Collections.Generic;
using System.Threading.Tasks;
using FoosballXamarin.Models;
using FoosballXamarin.Models.Dtos;
using Models;

namespace FoosballXamarin.Services
{
    public interface IPlayerService
    {
        Task<List<User>> GetDataAsync();
        Task<GetPlayerSeasonHistoryResponse> GetPlayerSeasonHistory(string email);
        Task<bool> CreateUser(string email, string displayName, string password);
        Task<List<PartnerPercentResult>> GetPartnerPercentResult(string email);
    }
}