using System.Collections.Generic;
using System.Threading.Tasks;
using FoosballXamarin.Models;
using FoosballXamarin.Models.Dtos;
using FoosballXamarin.UWP.Models.Dtos;
using Models;

namespace FoosballXamarin.Services
{
    public interface IMatchService
    {
        Task<List<Match>> GetDataAsync();
        Task<List<Match>> GetPlayerMatches(string email);
        Task<bool> SubmitMatches(SaveMatchesRequest request);
    }
}