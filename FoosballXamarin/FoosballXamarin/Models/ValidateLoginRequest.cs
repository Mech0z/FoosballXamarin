namespace FoosballXamarin.Models
{
    public class ValidateLoginRequest
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string DeviceName { get; set; }
    }
}