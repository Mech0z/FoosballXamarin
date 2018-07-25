using System.Collections.Generic;

namespace FoosballXamarin.Models.Dtos
{
    public class ChangeUserRolesRequest
    {
        public string UserEmail { get; set; }
        public List<string> Roles { get; set; }
    }
}