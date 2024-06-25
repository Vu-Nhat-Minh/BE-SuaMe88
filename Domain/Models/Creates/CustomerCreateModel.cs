namespace Domain.Models.Creates
{
    public class CustomerCreateModel
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}