using System;
using System.Collections.Generic;

#nullable disable

namespace TaskPracticeOrder.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<ItemUnit> ItemUnits { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
