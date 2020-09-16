//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using GroceryHub.Data;
//using GroceryHub.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using GroceryHub.ViewModels;
//using System.IO;
//using Google.Cloud.Vision.V1;
//using System.Globalization;
//using Plurally;


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
using Microsoft.AspNetCore.Http;
using GroceryHub.ViewModels;
using System.IO;
using Google.Cloud.Vision.V1;
using System.Web.WebPages;
using System.Globalization;
using Plurally;

namespace GroceryHub.Controllers
{
    [Authorize(Roles = "Admin")]

    public class FlyerReadersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public Pluralizer pluralizationService = new Pluralizer(new CultureInfo("en-us"));

        public FlyerReadersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FlyerReaders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FlyerReader.Include(f => f.Store);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FlyerReaders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flyerReader = await _context.FlyerReader
                .Include(f => f.Store)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flyerReader == null)
            {
                return NotFound();
            }

            return View(flyerReader);
        }

        // GET: FlyerReaders/Create
        public IActionResult Create()
        {
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreName");
            return View();
        }

        // POST: FlyerReaders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlyerReaderViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<FlyerOffer> flyerOffers = new List<FlyerOffer>();
                FlyerReader flyerReader = new FlyerReader
                {
                    Img = new List<ByteWraper>(),
                    DueDate = model.DueDate,
                    StartDate = model.StartDate,
                    StoreID = model.StoreID,
                    Data = new String(String.Empty)
                };

                if (model.photos.Count > 0)
                {
                    var data = String.Empty;
                    var flyerText = String.Empty;
                    foreach (IFormFile photo in model.photos)
                    {
                        var Stream = new MemoryStream();
                        await photo.CopyToAsync(Stream);
                        ByteWraper byteWraper = new ByteWraper();
                        byteWraper.photo = Stream.ToArray();
                        flyerReader.Img.Add(byteWraper);

                        var Googleimage = Image.FromBytes(byteWraper.photo);
                        var client = ImageAnnotatorClient.Create();
                        var response = client.DetectText(Googleimage);
                        foreach (var annotation in response)
                        {
                            if (annotation.Description != null)
                            {
                                flyerText += " " + annotation.Description;
                            }
                        }
                        String text = String.Empty;
                        text = flyerText.Replace("\n", " ");

                        string str = text.ToLower();
                        string[] arr = str.Split(" ");
                        var a =
                        from k in arr
                        orderby k
                        select k;
                        String Description = String.Empty;
                        String ItemName = String.Empty;
                        List<DoubleWraper> numbers = new List<DoubleWraper>();
                        foreach (string res in a.Distinct())
                        {
                            Description += " " + res.ToLower();
                            if (res != "")
                            {
                                Item x;
                                if (pluralizationService.IsPlural(res))
                                {
                                    x = _context.Item.FirstOrDefault(i => i.ItemName.ToLower() == pluralizationService.Singularize(res));
                                }
                                else
                                {
                                    x = _context.Item.FirstOrDefault(i => i.ItemName.ToLower() == res);
                                }

                                if (x != null)
                                {
                                    ItemName = x.ItemName;
                                }
                                if (res.IsInt() || res.IsDecimal())
                                {
                                    numbers.Add(new DoubleWraper { num = Double.Parse(res) });
                                }
                            }
                        }

                        if (numbers.Count > 0 && ItemName != "")
                        {
                            flyerOffers.Add(new FlyerOffer
                            {
                                ItemName = ItemName,
                                Description = Description,
                                Numbers = numbers,
                                flyerphoto = byteWraper.photo,
                                StoreID = flyerReader.StoreID,
                                DueDate = flyerReader.DueDate,
                                StartDate = flyerReader.StartDate,
                                flyerReader = flyerReader,
                                recognized = true
                            });
                        }
                        else if (ItemName == null || ItemName == "")
                        {
                            flyerOffers.Add(new FlyerOffer
                            {
                                Description = Description,
                                Numbers = numbers,
                                flyerphoto = byteWraper.photo,
                                StoreID = flyerReader.StoreID,
                                DueDate = flyerReader.DueDate,
                                StartDate = flyerReader.StartDate,
                                flyerReader = flyerReader,
                                recognized = false
                            });
                        }
                        flyerText = String.Empty;
                    }
                }
                flyerReader.flyerOffers = flyerOffers;
                _context.FlyerReader.Add(flyerReader);
                _context.SaveChanges();
                return RedirectToAction("FlyersOffers");
            }
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreName", model.StoreID);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> FlyersOffers()
        {
            var model = await _context.FlyerReader.Include(a => a.flyerOffers).ThenInclude(n => n.Numbers)
                .OrderByDescending(p => p.Id).FirstOrDefaultAsync();

            ViewBag.id = model.Id;
            return View("FlyersOffers", model.flyerOffers.Where(t => t.recognized == true));
        }

        [HttpPost]
        public async Task<JsonResult> FlyersOffers(String ItemName, FlyerOffer model)
        {

            Item item;
            if (pluralizationService.IsPlural(ItemName))
            {
                item = _context.Item.FirstOrDefault(i => i.ItemName.ToLower() == pluralizationService.Singularize(ItemName));
            }
            else
            {
                item = _context.Item.FirstOrDefault(i => i.ItemName.ToLower() == ItemName);
            }
            Store store = _context.Store.FirstOrDefault(s => s.StoreID == model.StoreID);

            Offer newOffer = new Offer
            {
                Item = item,
                ItemID = item.ItemID,
                OfferDescription = model.Description,
                DueDate = model.DueDate,
                StartDate = model.StartDate,
                Price = model.Price,
                IsValid = true,
                Store = store,
                StoreID = store.StoreID
            };

            _context.Add(newOffer);
            await _context.SaveChangesAsync();
            var result = new { Success = "True" };
            return Json(result);
        }



        [HttpGet]
        public async Task<IActionResult> UnrecognizedOffers(int? id)
        {
            var model = await _context.FlyerReader.Include(a => a.flyerOffers).ThenInclude(n => n.Numbers).OrderByDescending(p => p.Id).FirstOrDefaultAsync(a => a.Id == id);
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName");
            ViewBag.id = model.Id;

            return View("UnrecognizedOffers", model.flyerOffers.Where(t => t.recognized == false));
        }

        [HttpPost]
        public async Task<JsonResult> UnrecognizedOffers(String ItemName, int CategoryID, FlyerOffer model, IFormFile img)
        {
            Category category = _context.Category.FirstOrDefault(c => c.CategoryID == CategoryID);
            byte[] fromimage;
            var Stream = new MemoryStream();
            await img.CopyToAsync(Stream);
            fromimage = Stream.ToArray();

            if (pluralizationService.IsPlural(ItemName))
            {
                ItemName = pluralizationService.Singularize(ItemName);
            }

            Item item = new Item
            {
                Category = category,
                CategoryID = CategoryID,
                ItemName = ItemName,
                Image = fromimage
            };
            _context.Add(item);
            await _context.SaveChangesAsync();

            Store store = _context.Store.FirstOrDefault(s => s.StoreID == model.StoreID);
            Offer newOffer = new Offer
            {
                Item = item,
                ItemID = item.ItemID,
                OfferDescription = model.Description,
                DueDate = model.DueDate,
                StartDate = model.StartDate,
                Price = model.Price,
                IsValid = true,
                Store = store,
                StoreID = store.StoreID
            };

            _context.Add(newOffer);
            await _context.SaveChangesAsync();
            var result = new { Success = "True" };
            return Json(result);
        }

        public async Task<IActionResult> Finish(int? id)
        {
            var model = await _context.FlyerReader.Include(a => a.flyerOffers)
                .OrderByDescending(p => p.Id).FirstOrDefaultAsync();
            int items = model.flyerOffers.Where(t => t.recognized == false).Count();
            int offers = model.flyerOffers.Where(t => t.recognized == true).Count();
            String total = items + "$" + offers;
            return View("Finish", total);
        }


        // GET: FlyerReaders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flyerReader = await _context.FlyerReader.FindAsync(id);
            if (flyerReader == null)
            {
                return NotFound();
            }
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreName", flyerReader.StoreID);
            return View(flyerReader);
        }

        // POST: FlyerReaders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,StartDate,DueDate,StoreID")] FlyerReader flyerReader)
        {
            if (id != flyerReader.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flyerReader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlyerReaderExists(flyerReader.Id))
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
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreName", flyerReader.StoreID);
            return View(flyerReader);
        }

        // GET: FlyerReaders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flyerReader = await _context.FlyerReader
                .Include(f => f.Store)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flyerReader == null)
            {
                return NotFound();
            }

            return View(flyerReader);
        }

        // POST: FlyerReaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flyerReader = await _context.FlyerReader.FindAsync(id);
            _context.FlyerReader.Remove(flyerReader);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlyerReaderExists(int id)
        {
            return _context.FlyerReader.Any(e => e.Id == id);
        }
    }
}
