using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroceryHub.Data;
using GroceryHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryHub.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public CartsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cart.ToListAsync());
        }

        public async Task<IActionResult> MyCart(int pageNumber = 1)
        {
            AppUser user = await _userManager.GetUserAsync(HttpContext.User);

            var currentUser = _context.Users.
                Include(u => u.Cart).ThenInclude(s => s.CartItems).ThenInclude(o => o.Offer).ThenInclude(s => s.Store).
                Include(u => u.Cart).ThenInclude(s => s.CartItems).ThenInclude(o => o.Offer).ThenInclude(i => i.Item).
                First(i => i.UserName == user.UserName);

            return View(await PaginatedList<CartItem>.CreateAsync(_context.CartItem.Where(a => a.CartID == currentUser.CartID), pageNumber,5));
        }


        public async Task<JsonResult> RemoveFromCart(int? id)
        {
            AppUser user = await _userManager.GetUserAsync(HttpContext.User);

            var currentUser = _context.Users.
                Include(u => u.Cart).ThenInclude(s => s.CartItems).ThenInclude(o => o.Offer).ThenInclude(s => s.Store).
                Include(u => u.Cart).ThenInclude(s => s.CartItems).ThenInclude(o => o.Offer).ThenInclude(i => i.Item).
                First(i => i.UserName == user.UserName);

            CartItem a = currentUser.Cart.CartItems.Find(a => a.CartItemID == id);
            currentUser.Cart.CartItems.Remove(a);
            _context.Remove(a);

            IdentityResult result = await _userManager.UpdateAsync(currentUser);
            _context.SaveChanges();

            IEnumerable<CartItem> applicationDbContext = currentUser.Cart.CartItems;

            return Json(new { success = true, msg = "Removed" });
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.CartID == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartID,AppUserID")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartID,AppUserID")] Cart cart)
        {
            if (id != cart.CartID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.CartID))
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
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.CartID == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.CartID == id);
        }
    }
}
