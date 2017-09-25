using System;
using System.Collections.Generic;
using FoosballXamarin.Helpers;

namespace Models
{
    public class Match : ObservableObject
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

        public bool HaveScore => MatchResult.Team1Score != 0 || MatchResult.Team2Score != 0;

        public bool IsValid
        {
            get
            {
                //if(MatchResult.Team1Score == null && MatchResult.Team2Score != null)
                //{
                //    MatchValidationErrorText = "Both teams must have a score";
                //    return false;
                //}

                //if (MatchResult.Team2Score == null && MatchResult.Team1Score != null)
                //{
                //    MatchValidationErrorText = "Both teams must have a score";
                //    return false;
                //}

                if (MatchResult.Team1Score >= MatchResult.Team2Score + 2 ||
                    MatchResult.Team2Score >= MatchResult.Team1Score + 2)
                {
                }
                else
                {
                    MatchValidationErrorText = "A team must win with at least 2 points";
                    return false;
                }

                if (MatchResult.Team1Score >= 8 ||
                    MatchResult.Team2Score >= 8)
                {
                }
                else
                {
                    MatchValidationErrorText = "Winning team must have at least 8 points";
                    return false;
                }

                return true;
            }
        }

        public string MatchValidationErrorText { get; set; }
    }
}