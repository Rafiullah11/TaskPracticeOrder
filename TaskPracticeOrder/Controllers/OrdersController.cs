using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskPracticeOrder.Data;
using TaskPracticeOrder.Models;
using TaskPracticeOrder.Models.ViewModels;

namespace TaskPracticeOrder.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["ItemNameSorting"] = String.IsNullOrEmpty(sortOrder) ? "itemName_desc" : "";
            ViewData["PriceSorting"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["DataFilter"] = searchString;

            var data = (from Item in _context.Items
                        join UnitItem in _context.UnitItems on Item.ItemId equals UnitItem.ItemId
                        join Unit in _context.Units on UnitItem.UnitId equals Unit.UnitId
                        select new OrderViewModel
                        {
                            ItemName = Item.ItemName,
                            UnitType = Unit.UnitType,
                            Price = Item.Price,
                            ItemId = Item.ItemId,
                            UnitId = Unit.UnitId

                        });
            if (!String.IsNullOrEmpty(searchString))
            {
                data = data.Where(s => s.ItemName.Contains(searchString)
                                       || s.UnitType.Contains(searchString));
            }
            data = sortOrder switch
            {
                "itemName_desc" => data.OrderByDescending(s => s.ItemName),
                "Price" => data.OrderBy(s => s.Price),
                "price_desc" => data.OrderByDescending(s => s.Price),
                _ => data.OrderBy(s => s.ItemName),
            };
            return View(await data.ToListAsync()); ;

        }
        public IActionResult Create(int? id)
        {
            var items = (from item in _context.Items
                         join UnitItem in _context.UnitItems on item.ItemId equals UnitItem.ItemId
                         join unit in _context.Units on UnitItem.UnitId equals unit.UnitId
                         //join OrderItem in _context.OrderItems on UnitItem.OrderItemId equals OrderItem.OI_Id
                         select new OrderViewModel
                         {
                             ItemId = item.ItemId,
                             UnitId = unit.UnitId,
                             UnitType = unit.UnitType,
                             ItemName = item.ItemName,
                             Price = item.Price,
                             
                         }).ToList();

            return View(items);
        }

        [HttpPost]
        public async Task< IActionResult> Create(List<OrderViewModel> orderVM, int id)
        {
            //var order =  _context.Orders
            //  .Include(o => o.OrderItems)
            //  .Include(o => o.OrderItems)
            //  .FirstOrDefaultAsync(m => m.OrderId == id);

            var createOrder = new Order()
            {
                OrderDate = DateTime.UtcNow,
            };
            //var checkQuantity = orderVM.Select(x => x.Quantity).ToList();

            var checkQty = orderVM.Where(x => x.Quantity ==x.Quantity).Count();

            var selectedChk = orderVM.Select(x => x.ItemId).Count();

            if (checkQty != null && selectedChk > 0)
            {

                foreach (var item in orderVM)
                {
                    var UnitItemID = _context.UnitItems.Where(x => x.ItemId == item.ItemId).FirstOrDefault();

                    createOrder.OrderItems.Add(new OrderItem()
                    {
                        ItemUnitID = UnitItemID.Id,
                        Quantity = item.Quantity,
                        Price = item.Price * item.Quantity,
                        OrderItemID = createOrder.OrderId,
                    });
                    await _context.AddAsync(createOrder);
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            //var sa= _context.Orders.Include(x=>x.OrderItems).ThenInclude(y=>y.UnitItem).ThenInclude()

            var orders = (from order in _context.Orders
                         join OrderItem in _context.OrderItems on order.OrderId equals OrderItem.OrderItemID
                         join UnitItem in _context.UnitItems on OrderItem.ItemUnitID equals UnitItem.ItemId
                         join unit in _context.Units on UnitItem.UnitId equals unit.UnitId
                         join Item in _context.Items on UnitItem.ItemId equals Item.ItemId
                       select new OrderViewModel
                         {   
                             ItemId = Item.ItemId,
                             UnitId = unit.UnitId,
                             UnitType = unit.UnitType,
                             ItemName = Item.ItemName,
                             Price = OrderItem.Price,
                             ItemUnitID=OrderItem.ItemUnitID,
                             OrderItemID=OrderItem.OrderItemID,
                             Quantity=OrderItem.Quantity,
                             OrderId=order.OrderId
                         }).ToList();

            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Order order)
        {
           
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View();

        }


    }
}
