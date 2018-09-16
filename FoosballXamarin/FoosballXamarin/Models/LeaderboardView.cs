using System;
using System.Collections.Generic;

namespace FoosballXamarin.Models
{
    public class LeaderboardView : ICloneable
    {
        public LeaderboardView()
        {
            Id = Guid.NewGuid();
            Entries = new List<LeaderboardViewEntry>();
            Timestamp = DateTime.UtcNow;
        }
        
        public Guid Id { get; set; }
        public List<LeaderboardViewEntry> Entries { get; set; }
        public string SeasonName { get; set; }
        public DateTime? Timestamp { get; set; }
        public DateTime StartDate { get; set; }

        public object Clone()
        {
            return new LeaderboardView
            {
                Entries = Entries,
                Id = Id,
                SeasonName = SeasonName,
                Timestamp = Timestamp
            };
        }
    }
}