using System.Collections.Generic;
using FoosballXamarin.UWP.Models.Dtos;

namespace FoosballXamarin.Models.Dtos
{
    public class ChangeUserRolesRequest : BaseRequest
    {
        public string UserEmail { get; set; }
        public List<string> Roles { get; set; }
    }
}