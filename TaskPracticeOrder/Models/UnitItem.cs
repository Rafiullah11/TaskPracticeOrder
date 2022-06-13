using System.Collections.Generic;

namespace TaskPracticeOrder.Models
{
    public class UnitItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int UnitId { get; set; }
        public int OrderItemId { get; set; }
        public virtual Item Item { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
