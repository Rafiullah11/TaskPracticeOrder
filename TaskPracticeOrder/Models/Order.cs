using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskPracticeOrder.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual ICollection<OrderItem>  OrderItems { get; set; }=new HashSet<OrderItem>();
    }
}