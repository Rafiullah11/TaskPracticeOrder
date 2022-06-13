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
    public class ItemsController : Controller
    {
        private readonly AppDbContext _context;

        public ItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["ItemNameSorting"] = String.IsNullOrEmpty(sortOrder) ? "itemName_desc" : "";
            ViewData["PriceSorting"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["DataFilter"] = searchString;

            var data = (from Item in _context.Items
                        join UnitItem in _context.UnitItems on Item.ItemId equals UnitItem.ItemId
                        join Unit in _context.Units on UnitItem.UnitId equals Unit.UnitId
                        select new ItemUnitViewModel
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

        // GET: Items/Create
        public IActionResult Create(int? id)
        {

            var item = new ItemViewModel();
                item.UnitItems = _context.UnitItems.ToList();
                ViewBag.unitDD = new SelectList(_context.Units, "UnitId", "UnitType");

                return View(item);
        }

        // POST: Items/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemViewModel vm, int? id)
        {
                Item post = new Item()
                {
                    ItemId = vm.ItemId,
                    ItemName = vm.ItemName,
                    Price = vm.Price,
                    UnitItems = vm.UnitItems,
                };
                if (ModelState.IsValid)
                {
                    _context.Add(post);
                    await _context.SaveChangesAsync();
                }

                foreach (var unt in vm.SelectedUnit)
                {
                    UnitItem unitItem = new UnitItem();
                    unitItem.ItemId = post.ItemId;
                    unitItem.UnitId = unt;
                    _context.Add(unitItem);
                    await _context.SaveChangesAsync();
                }
                TempData["message"] = "Successfully Added";
                return RedirectToAction("Index");
            
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var item = await  _context.Items
                .Include(p => p.UnitItems)
                .Where(p => p.ItemId == id)
                .FirstOrDefaultAsync();
            if (item == null) return View();

            var itemVM = new ItemViewModel()
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                Price = item.Price,
                UnitItems = _context.UnitItems.ToList(),
                SelectedUnit = item.UnitItems.Select(pc => pc.UnitId).ToList()

            };
            ViewBag.unitDD = new SelectList(_context.Units, "UnitId", "UnitType");
            return View(itemVM);
        }

        //POST: Items/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ItemViewModel vm)
        {
            if (id != vm.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                    var post = await _context.Items.Include(p => p.UnitItems)
                                             .Where(p => p.ItemId == id)
                                             .FirstOrDefaultAsync();

                    post.ItemName = vm.ItemName;
                    post.Price = vm.Price;
                    post.UnitItems = new List<UnitItem>();

                    foreach (var unitId in vm.SelectedUnit)
                    {
                        post.UnitItems.Add(new UnitItem { UnitId = unitId });
                    }
                    await _context.SaveChangesAsync();
                    TempData["Editmessage"] = "Edited Successfully";
                    return RedirectToAction("Index");
                
            }
            return View(vm);
        }

        //GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.UnitItems)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }
    }
}
