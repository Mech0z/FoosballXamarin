using System;
using System.Collections.Generic;
using System.Linq;
using FoosballXamarin.UWP.Models.Dtos;

namespace FoosballXamarin.Models
{
    public class UserMapping
    {
        public UserMapping(GetUserMappingsResponseEntry entry)
        {
            Email = entry.Email;
            Roles = entry.Roles;
            if (Roles == null)
            {
                Roles = new List<string>();
            }
        }

        public string DisplayName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get;set; }
        public string RolesAsString => String.Join(", ", Roles);
    }
}