using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace FoosballXamarin.Services
{
    public interface IMatchService
    {
        Task<List<Match>> GetDataAsync();
        Task<List<Match>> GetPlayerMatches();
    }
}