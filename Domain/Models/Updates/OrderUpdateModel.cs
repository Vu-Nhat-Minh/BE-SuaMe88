using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Updates
{
    public class OrderStatusUpdateModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string? Status { get; set; }
    }
}
