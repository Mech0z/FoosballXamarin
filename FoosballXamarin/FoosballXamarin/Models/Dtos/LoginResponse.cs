using System;
using System.Collections.Generic;

namespace FoosballXamarin.UWP.Models.Dtos
{
    public class LoginResponse
    {
        public DateTime ExpiryTime { get; set; }
        public string Token { get; set; }
        public bool LoginFailed { get; set; }
        public List<string> Roles { get; set; }
    }
}