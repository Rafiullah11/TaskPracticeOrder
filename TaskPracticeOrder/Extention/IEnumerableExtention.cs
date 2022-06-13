using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace TaskPracticeOrder.Extentions
{
    public static class IEnumerableExtention
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> Items)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            SelectListItem Sli = new SelectListItem();
            //{
            //    Text = "---Select Unit Type---",
            //    Value = "0"
            //};
            //list.Add(Sli);
            foreach (var item in Items)
            {
                Sli = new SelectListItem()
                {
                  // Text = item.GetType().GetProperty("Name").GetValue(item, null).ToString(),
                  // Value = item.GetType().GetProperty("Id").GetValue(item, null).ToString()
                    Text=item.GetPropertyValue("UnitType"),
                    Value=item.GetPropertyValue("UnitId")

                };
                list.Add(Sli);
            }
            return list;
        }
    }
}
