using System;
using System.Collections.Generic;

#nullable disable

namespace TaskPracticeOrder.Models

{
    public  class Unit
    {
        public int UnitId { get; set; }
        public string UnitType { get; set; }

        public virtual ICollection<ItemUnit> ItemUnits { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

    }
}
