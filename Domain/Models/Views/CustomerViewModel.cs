namespace Domain.Models.Views
{
    public class CustomerViewModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public int Point { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreateAt { get; set; }
    }
}
