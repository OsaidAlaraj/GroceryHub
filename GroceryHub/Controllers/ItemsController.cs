using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroceryHub.Data;
using GroceryHub.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

using System.Globalization;
using Plurally;

namespace GroceryHub.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public Pluralizer pluralizationService = new Pluralizer(new CultureInfo("en-us"));

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public void MyPluralizer()
        {
            List<Item> items = _context.Item.ToList();
            foreach(Item item in items)
            {
                if (pluralizationService.IsPlural(item.ItemName))
                {
                    item.ItemName = pluralizationService.Singularize(item.ItemName);
                    _context.Item.Update(item);
                    _context.SaveChanges();
                }
            }
        }

        // GET: Items
        public async Task<IActionResult> Index(int? id,int pageNumber =1)
        {
            if (id != null)
            {
                return View(await PaginatedList<Item>.CreateAsync(_context.Item.
                Include(o => o.Category).Where(a => a.CategoryID == id), pageNumber, 18));
            }
            else
            {
                return View(await PaginatedList<Item>.CreateAsync(_context.Item.Include(i => i.Category), pageNumber, 18));
            }
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
        [Authorize(Roles = "Admin")]
        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ItemID,ItemName,Image,CategoryID")] Item item, IFormFile img)
        {
            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    var Stream = new MemoryStream();
                    await img.CopyToAsync(Stream);
                    item.Image = Stream.ToArray();
                }
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName", item.CategoryID);
            return View(item);
        }
        [Authorize(Roles = "Admin")]
        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName", item.CategoryID);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ItemID,ItemName,Image,CategoryID")] Item item,IFormFile img)
        {
            if (id != item.ItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (img != null)
                    {
                        var Stream = new MemoryStream();
                        await img.CopyToAsync(Stream);
                        item.Image = Stream.ToArray();
                    }
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName", item.CategoryID);
            return View(item);
        }

        // GET: Items/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Item.FindAsync(id);
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ItemID == id);
        }
    }
}
