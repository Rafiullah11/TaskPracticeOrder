using System;
using System.Collections.Generic;

#nullable disable

namespace TaskPracticeOrder.Models

{
    public class ItemUnit
    {
        public int ItemId { get; set; }
        public int UnitId { get; set; }

        public virtual Item Item { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
