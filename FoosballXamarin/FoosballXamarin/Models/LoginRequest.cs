﻿namespace FoosballXamarin.Models
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get;set; }
        public string DeviceName { get; set; }
    }
}