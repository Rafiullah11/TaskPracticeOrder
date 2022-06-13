using System.Collections.Generic;

namespace TaskPracticeOrder.Models
{
    public class Unit
    {
        public int UnitId { get; set; }
        public string UnitType { get; set; }
        public  ICollection<UnitItem> UnitItems { get; set; }

    }
}
