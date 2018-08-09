using System;
using System.Collections.Generic;

namespace FoosballXamarin.Models
{
    public class UserSettings
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryTime { get; set; }
        public List<string> Roles { get;set; }
    }
}
