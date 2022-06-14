using System;
using System.Collections.Generic;

#nullable disable

namespace TaskPracticeOrder.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime? Date { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}
