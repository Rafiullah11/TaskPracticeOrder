using System.ComponentModel.DataAnnotations;

namespace TaskPracticeOrder.Models
{
    public class OrderItem
    {
        [Key]
        public int OI_Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int ItemUnitID { get; set; }
        public int OrderItemID { get; set; }
        public decimal GrandTotal { get { return Quantity * Price; } }
        public virtual UnitItem UnitItem { get; set; }
        public virtual Order Order { get; set; }
    }
}
