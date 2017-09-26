namespace Models
{
    public class LeaderboardViewEntry
    {
        public int Rank { get; set; }
        public int NumberOfGames { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int EloRating { get; set; }
        public string Form { get; set; }
        public double WinPercent => (double)Wins / (Losses + Wins) * 100;
    }
}