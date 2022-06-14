using System.Collections.Generic;

namespace TaskPracticeOrder.Models.ViewModels
{
    public class ItemViewModel
    {
        public int ItemId { get; set; }
        public int UnitId { get; set; }
        public string ItemName { get; set; }
        public int Price { get; set; }
        public List<int> SelectedUnit { get; set; }

        public ICollection<ItemUnit> ItemUnits { get; set; }
    }
}
