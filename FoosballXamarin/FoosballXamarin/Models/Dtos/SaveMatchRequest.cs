using System.Collections.Generic;
using Models;

namespace FoosballXamarin.UWP.Models.Dtos
{
    public class SaveMatchesRequest
    {
        public User User { get; set; }
        public List<Match> Matches { get; set; }
    }
}