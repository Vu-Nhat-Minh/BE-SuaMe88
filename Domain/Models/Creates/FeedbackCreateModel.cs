﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Creates
{
    public class FeedbackCreateModel
    {
        public Guid productId { get; set; }
        public string? Message { get; set; }
        public int Star { get; set; }
    }
}
