using System.Collections.Generic;
using System.Threading.Tasks;
using FoosballXamarin.UWP.Models.Dtos;
using Models;

namespace FoosballXamarin.Services
{
    public interface IMatchService
    {
        Task<List<Match>> GetDataAsync();
        Task<List<Match>> GetPlayerMatches();
        Task<bool> SubmitMatches(SaveMatchesRequest request);
    }
}