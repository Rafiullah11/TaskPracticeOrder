using System.Collections.Generic;

namespace TaskPracticeOrder.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Price { get; set; }

        public  ICollection<UnitItem> UnitItems { get; set; }=new HashSet<UnitItem>();


    }
}
