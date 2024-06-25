namespace Domain.Models.Authentications
{
    public class AuthModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public string Role { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
