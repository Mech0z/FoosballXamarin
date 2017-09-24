using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class Match
    {
        public bool ScoreIsHighEnough => MatchResult.Team1Score >= 8 || MatchResult.Team2Score >= 8;

        public Match()
        {
            UserList = new List<User>();
            MatchResult = new MatchResult();
        }
        
        public Guid Id { get; set; }

        public DateTime TimeStampUtc { get; set; }
        public List<string> PlayerList { get; set; }

        public List<User> UserList { get; set; }

        public MatchResult MatchResult { get; set; }

        public int? Points { get; set; }

        public String SeasonName { get; set; }

        public bool IsValid
        {
            get
            {

                if (MatchResult.Team1Score >= MatchResult.Team2Score + 2 ||
                    MatchResult.Team2Score >= MatchResult.Team1Score + 2)
                {
                    MatchValidationErrorText = "A team must win with at least 2 points";
                }
                else
                {
                    return false;
                }

                if (MatchResult.Team1Score >= 8 ||
                    MatchResult.Team2Score >= 8)
                {
                    MatchValidationErrorText = "Winning team must have at least 8 points";
                }
                else
                {
                    return false;
                }

                return true;
            }
        }

        public string MatchValidationErrorText { get; set; }
    }
}