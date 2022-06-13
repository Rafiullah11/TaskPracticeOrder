using System;
using System.Collections.Generic;

namespace TaskPracticeOrder.Models.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<OrderItem> orderItems { get; set; }
        public ICollection<UnitItem> UnitItems { get; set; }
        public int ItemId { get; set; }
        public DateTime OrderDate { get; set; }
        public int UnitId { get; set; }
        public int ItemUnitID { get; set; }
        public int OrderId { get; set; }

        public int OrderItemID { get; set; }
        public string ItemName { get; set; }
        public string UnitType { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        //public decimal GrandTotal { get { return Quantity * Price; } }
        public decimal? GrandTotal { get; set; }
        //public List<int> SelectedUnit { get; set; }

    }
}
