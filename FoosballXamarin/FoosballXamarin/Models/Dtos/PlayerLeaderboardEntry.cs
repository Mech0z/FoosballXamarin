namespace FoosballXamarin.Models.Dtos
{
    public class PlayerLeaderboardEntry
    {
        public string SeasonName { get; set; }
        public int Rank { get; set; }
        public int NumberOfGames { get; set; }
        public string UserName { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int EloRating { get; set; }

        public string DisplayGamesDistribution => $"{Wins}/{NumberOfGames}";
        public double WinPercent => (double)Wins / (double)NumberOfGames * 100;
        public string WinPercentDisplay => WinPercent + "%";
    }
}