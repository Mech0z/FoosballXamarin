using System;
using FoosballXamarin.Helpers;
using FoosballXamarin.UWP.Models.Dtos;

namespace FoosballXamarin.Models
{
    public class UserMapping
    {
        public UserMapping(GetUserMappingsResponseEntry entry)
        {
            Email = entry.Email;
            Roles = new ObservableRangeCollection<string>();
            if (entry.Roles != null)
            {
                Roles.AddRange(entry.Roles);
            }
        }

        public string DisplayName { get; set; }
        public string Email { get; set; }
        public ObservableRangeCollection<string> Roles { get;set; }
        public string RolesAsString => String.Join(", ", Roles);
    }
}