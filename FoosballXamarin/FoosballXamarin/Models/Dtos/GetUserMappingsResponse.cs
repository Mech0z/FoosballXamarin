using System.Collections.Generic;

namespace FoosballXamarin.UWP.Models.Dtos
{
    public class GetUserMappingsResponse
    {
        public List<GetUserMappingsResponseEntry> Users { get;set; }
    }
}