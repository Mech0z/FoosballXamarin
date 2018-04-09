using System.Collections.Generic;
using Models;

namespace FoosballXamarin.UWP.Models.Dtos
{
    public class SaveMatchesRequest : BaseRequest
    {
        public List<Match> Matches { get; set; }
    }
}