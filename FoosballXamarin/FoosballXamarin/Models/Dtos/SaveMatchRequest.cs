using System.Collections.Generic;

namespace FoosballXamarin.Models.Dtos
{
    public class SaveMatchesRequest
    {
        public List<Match> Matches { get; set; }
        public string Email { get; set; }
    }
}