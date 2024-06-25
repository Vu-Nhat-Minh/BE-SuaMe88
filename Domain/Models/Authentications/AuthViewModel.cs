using Domain.Models.Views;

namespace Domain.Models.Authentications
{
    public class AuthViewModel
    {
        public string AccessToken { get; set; } = null!;
        public UserViewModel User { get; set; } = null!;
    }
}
