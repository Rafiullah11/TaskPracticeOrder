using System.Collections.Generic;
using TaskPracticeOrder.Models;

namespace TaskPracticeOrder.Models.ViewModels
{
    public class ItemUnitViewModel
    {
        public string ItemName { get; set; }
        public string UnitType { get; set; }
        public int ItemId { get; set; }
        public int UnitId { get; set; }
        public decimal Price { get; set; }
  
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<Unit> Units { get; set; }

    }
}
