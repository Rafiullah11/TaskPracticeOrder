using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace TaskPracticeOrder.Models

{
    public class OrderItem
    {
        [Key]
        public int UnitId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public virtual Item Item { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual Order Order { get; set; }
    }
}
