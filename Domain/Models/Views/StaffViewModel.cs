namespace Domain.Models.Views
{
    public class StaffViewModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime CreateAt { get; set; }
    }
}
