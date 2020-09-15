using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroceryHub.Data;
using GroceryHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GroceryHub.Controllers
{
    public class OffersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public OffersController(ApplicationDbContext context, UserManager<AppUser> userManager )
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Offers
        public async Task<IActionResult> Index(int? id, string? val, int pageNumber =1)
        {
            ValidateOffers();
            OfferRatingSystem();
            if (val == "Store")
            {
                var applicationDbContext = _context.Offer.Include(o => o.Item)
                    .Include(o => o.Store).Where(a => a.StoreID == id)
                    .Where(o => o.IsValid == true);

                return View("Index", await PaginatedList<Offer>.CreateAsync(applicationDbContext, pageNumber, 18));
            }
            else if(val == "Item")
            {
                if (_context.Offer.Where(i => i.ItemID == id).Count() > 0 && _context.Offer.Where(i => i.ItemID == id) != null)
                {
                    ViewBag.BestPrice = _context.Offer.Where(i=>i.ItemID == id).Include(x=>x.Item).OrderBy(a => a.Price).First();
                }

                return View("Index", await PaginatedList<Offer>.CreateAsync(_context.Offer.Include(o => o.Item)
                    .Include(o => o.Store).Where(a => a.ItemID == id)
                    .Where(o => o.IsValid == true), pageNumber, 18));
            }
            else
            {
                return View(await PaginatedList<Offer>.CreateAsync(_context.Offer.Include(o => o.Item)
                    .Include(o => o.Store).Where(a => a.IsValid == true), pageNumber, 18));
            }
        }

        public void OfferRatingSystem()
        {
            foreach(Item item in _context.Item)
            {
                if (item.Offers != null)
                {
                    var avg = item.Offers.Select(x => x.Price).DefaultIfEmpty(0).Average();
                    var lowestPrice = item.Offers.OrderBy(i => i.Price).FirstOrDefault().Price;
                    var highstPrice = item.Offers.OrderByDescending(i => i.Price).FirstOrDefault().Price;

                    foreach (Offer offer in item.Offers)
                    {
                        if (offer.Price >= highstPrice)
                        {
                            offer.Level = 1;
                            _context.Update(offer);
                        }
                        else if (offer.Price > avg && offer.Price < highstPrice)
                        {
                            offer.Level = 2;
                            _context.Update(offer);
                        }
                        else if (offer.Price == avg)
                        {
                            offer.Level = 3;
                            _context.Update(offer);
                        }
                        else if (offer.Price < avg && offer.Price > lowestPrice)
                        {
                            offer.Level = 4;
                            _context.Update(offer);
                        }
                        else if (offer.Price <= lowestPrice)
                        {
                            offer.Level = 5;
                            _context.Update(offer);
                        }
                    }
                }
            }
            _context.SaveChanges();
        }

        [Authorize]
        public async Task<JsonResult> AddToCart(int? id)
        {
            if (id == 0)
            {
                return Json(new { success = true, msg = "Exist" });

            }
            Offer offer = await _context.Offer.Include(o => o.cartItems)
                .Include(i => i.Item).Include(s => s.Store)
                .FirstOrDefaultAsync(m => m.OfferID == id);

            AppUser user = await _userManager.GetUserAsync(HttpContext.User);

            var currentUser = _context.Users.Include(u => u.Cart).
                ThenInclude(m=>m.CartItems).ThenInclude(o => o.Offer).First(i => i.UserName == user.UserName);

            if (currentUser.Cart == null)
            {
                Cart tempCart = new Cart
                {
                    AppUser = currentUser,
                    AppUserID = currentUser.Id,
                    CartItems = new List<CartItem>()
                };

                _context.Cart.Add(tempCart);
                _context.SaveChanges();

                currentUser.CartID = tempCart.CartID;
                currentUser.Cart = tempCart;
                IdentityResult result1 = await _userManager.UpdateAsync(currentUser);
                _context.SaveChanges();
            }

            if (currentUser.Cart.CartItems.FirstOrDefault(o => o.Offer.OfferID == offer.OfferID) != null)
            {
                ViewBag.Message = "Exist";
                return Json(new { success = true, msg = "Exist" });

            }

            else
            {
                CartItem cartItem = new CartItem
                {
                    Cart = currentUser.Cart,
                    CartID = currentUser.Cart.CartID,
                    Offer = offer,
                    OfferID = offer.OfferID
                };

                currentUser.Cart.CartItems.Add(cartItem);
                offer.cartItems.Add(cartItem);

                _context.CartItem.Add(cartItem);
                IdentityResult result = await _userManager.UpdateAsync(currentUser);
                _context.SaveChanges();
                ViewBag.Message = "Added";
                return Json(new { success = true, msg = "Added" });
            }
        }

        // GET: Offers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offer
                .Include(o => o.Item)
                .Include(o => o.Store)
                .FirstOrDefaultAsync(m => m.OfferID == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }
        [Authorize(Roles = "Admin")]
        // GET: Offers/Create
        public IActionResult Create()
        {
            ViewData["ItemID"] = new SelectList(_context.Item, "ItemID", "ItemName");
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreName");
            return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("OfferID,Unit,Price,StartDate,DueDate,Level,OfferDescription,IsValid,StoreID,ItemID")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                bool a=offer.IsValid;
                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemID"] = new SelectList(_context.Item, "ItemID", "ItemName", offer.ItemID);
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreName", offer.StoreID);
            return View(offer);
        }




        // GET: Offers/Edit/5    
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offer.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            ViewData["ItemID"] = new SelectList(_context.Item, "ItemID", "ItemName", offer.ItemID);
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreName", offer.StoreID);
            return View(offer);
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("OfferID,Unit,Price,StartDate,DueDate,Level,OfferDescription,IsValid,StoreID,ItemID")] Offer offer)
        {
            if (id != offer.OfferID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(offer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferExists(offer.OfferID))
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
            ViewData["ItemID"] = new SelectList(_context.Item, "ItemID", "ItemName", offer.ItemID);
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreName", offer.StoreID);
            return View(offer);
        }

        // GET: Offers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offer
                .Include(o => o.Item)
                .Include(o => o.Store)
                .FirstOrDefaultAsync(m => m.OfferID == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var offer = await _context.Offer.FindAsync(id);
            _context.Offer.Remove(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfferExists(int id)
        {
            return _context.Offer.Any(e => e.OfferID == id);
        }

        public void ValidateOffers()
        {
            DateTime now = DateTime.Now;
            foreach (Offer offer in _context.Offer.ToList())
            {
                if (offer.Price == 0)
                {
                    _context.Offer.Remove(offer);
                }
                if (offer.DueDate < now) 
                {
                    offer.IsValid = false;
                }
                if (offer.DueDate > now)
                {
                    offer.IsValid = true;
                }
            }
            _context.SaveChanges();
        }
    }
}
