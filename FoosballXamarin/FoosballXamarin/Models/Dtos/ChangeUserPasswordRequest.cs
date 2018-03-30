using FoosballXamarin.UWP.Models.Dtos;

namespace FoosballXamarin.Models.Dtos
{
    public class ChangeUserPasswordRequest : BaseRequest
    {
        public string UserEmail { get; set; }
        public string NewPassword { get; set; }
    }
}