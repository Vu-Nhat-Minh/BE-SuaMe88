using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Views
{
    public class FeedbackViewModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? Message { get; set; }
        public int Star { get; set; }
        public DateTime CreateAt { get; set; }
        public virtual CustomerViewModel Customer { get; set; } = null!;
    }
}
