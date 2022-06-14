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
                        join UnitItem in _context.ItemUnits on Item.ItemId equals UnitItem.ItemId
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
                         join UnitItem in _context.ItemUnits on item.ItemId equals UnitItem.ItemId
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
        public async Task<IActionResult> Create(List<OrderViewModel> orderViewModel, int id)
        {
            var order = new Order();
            order.Date = DateTime.Now;

            var checkQty = orderViewModel.Where(x => x.Quantity == x.Quantity).Count();

            var selectedChk = orderViewModel.Select(x => x.ItemId).Count();

            if (checkQty != null && selectedChk > 0)
            {

                foreach (var item in orderViewModel)
                {
                    var unitItemId = _context.ItemUnits.Where(x => x.ItemId == item.ItemId).FirstOrDefault();


                    order.OrderItems.Add(new OrderItem()
                    {
                        ItemId = item.ItemId,
                        UnitId = item.UnitId,
                        Quantity = item.Quantity,
                        OrderId = order.OrderId,
                    });
                    await _context.AddAsync(order);
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(List<OrderViewModel> orderViewModel, int? id)
        {

            var orders = (from order in _context.Orders
                          join OrderItem in _context.OrderItems on order.OrderId equals OrderItem.OrderId
                          join unit in _context.Units on OrderItem.UnitId equals unit.UnitId
                          join Item in _context.Items on OrderItem.ItemId equals Item.ItemId
                          select new OrderViewModel
                          {
                              ItemId = Item.ItemId,
                              UnitId = unit.UnitId,
                              UnitType = unit.UnitType,
                              ItemName = Item.ItemName,
                              Price = Item.Price,

                              OrderItemID = OrderItem.ItemId,
                              Quantity = OrderItem.Quantity,
                              OrderId = order.OrderId
                          }).ToList();

            return View( orders);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(List<OrderViewModel> orderViewModel, int id)
        {
            if (orderViewModel.Count() == 0)
            {
                return NotFound();
            }
            var order = new Order();
            order.Date = DateTime.Now;

            var checkQty = orderViewModel.Where(x => x.Quantity == x.Quantity).Count();

            var selectedChk = orderViewModel.Select(x => x.ItemId).Count();

            if (checkQty != null && selectedChk > 0)
            {
                foreach (var item in orderViewModel)
                {
                    var unitItemId = _context.ItemUnits.Where(x => x.ItemId == item.ItemId).FirstOrDefault();

                    order.OrderItems.Add(new OrderItem()
                    {
                        ItemId = item.ItemId,
                        UnitId = item.UnitId,
                        Quantity = item.Quantity,
                        OrderId = order.OrderId,
                    });
                    _context.Update(order);
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
