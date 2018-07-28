using System.Collections.Generic;

namespace FoosballXamarin.Models.Dtos
{
    public class GetPlayerSeasonHistoryResponse
    {
        public GetPlayerSeasonHistoryResponse()
        {
            PlayerLeaderboardEntries = new List<PlayerLeaderboardEntry>();
        }

        public List<PlayerLeaderboardEntry> PlayerLeaderboardEntries { get; set; }
    }
}