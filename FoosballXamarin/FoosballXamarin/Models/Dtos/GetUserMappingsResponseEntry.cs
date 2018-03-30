using System.Collections.Generic;

namespace FoosballXamarin.UWP.Models.Dtos
{
    public class GetUserMappingsResponseEntry
    {
        public string Email { get; set; }
        public List<string> Roles { get;set; }
    }
}