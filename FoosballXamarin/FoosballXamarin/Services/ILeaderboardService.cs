using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace FoosballXamarin.Services
{
    public interface ILeaderboardService
    {
        Task<List<LeaderboardView>> GetDataAsync();
    }
}