using System;

namespace FoosballXamarin.Models
{
    public class LoginResponse
    {
        public DateTime ExpiryTime { get; set; }
        public string Token { get; set; }
        public bool LoginFailed { get; set; }
    }
}