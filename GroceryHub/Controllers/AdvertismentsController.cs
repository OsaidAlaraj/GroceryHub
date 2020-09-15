using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GroceryHub.Data;
using GroceryHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryHub.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AdvertismentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdvertismentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Advertisments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Advertisment.ToListAsync());
        }

        // GET: Advertisments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisment = await _context.Advertisment
                .FirstOrDefaultAsync(m => m.AdvertismentID == id);
            if (advertisment == null)
            {
                return NotFound();
            }

            return View(advertisment);
        }

        // GET: Advertisments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Advertisments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdvertismentID,AdvertismentName,AdvertismentPhoto,AdvertismentDescription")] Advertisment advertisment, IFormFile img)
        {
            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    var Stream = new MemoryStream();
                    await img.CopyToAsync(Stream);
                    advertisment.AdvertismentPhoto = Stream.ToArray();
                }
                _context.Add(advertisment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(advertisment);
        }

        // GET: Advertisments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisment = await _context.Advertisment.FindAsync(id);
            if (advertisment == null)
            {
                return NotFound();
            }
            return View(advertisment);
        }

        // POST: Advertisments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdvertismentID,AdvertismentName,AdvertismentPhoto,AdvertismentDescription")] Advertisment advertisment, IFormFile img)
        {
            if (id != advertisment.AdvertismentID)
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
                        advertisment.AdvertismentPhoto = Stream.ToArray();
                    }
                    _context.Update(advertisment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvertismentExists(advertisment.AdvertismentID))
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
            return View(advertisment);
        }

        // GET: Advertisments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisment = await _context.Advertisment
                .FirstOrDefaultAsync(m => m.AdvertismentID == id);
            if (advertisment == null)
            {
                return NotFound();
            }

            return View(advertisment);
        }

        // POST: Advertisments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advertisment = await _context.Advertisment.FindAsync(id);
            _context.Advertisment.Remove(advertisment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdvertismentExists(int id)
        {
            return _context.Advertisment.Any(e => e.AdvertismentID == id);
        }
    }
}
