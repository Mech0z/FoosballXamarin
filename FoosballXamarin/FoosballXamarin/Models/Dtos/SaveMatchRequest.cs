using System.Collections.Generic;
using Models;

namespace FoosballXamarin.UWP.Models.Dtos
{
    public class SaveMatchesRequest
    {
        public List<Match> Matches { get; set; }
        public string Email { get; set; }
    }
}