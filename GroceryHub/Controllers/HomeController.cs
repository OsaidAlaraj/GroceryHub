using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GroceryHub.Models;
using GroceryHub.Data;
using GroceryHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GroceryHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel
            {
                HotOffers = await _context.Offer.Include(i => i.Item).Where(o => o.Level >= 3.0).
                    Where(v => v.IsValid == true).OrderByDescending(by => by.OfferID).
                        Take(10).ToListAsync(),

                LastAddedOffers = await _context.Offer.Include(i => i.Item).Where(v => v.IsValid == true).
                    OrderByDescending(by => by.OfferID).Take(10).ToListAsync(),

                TopRatedStores = await _context.Store.Where(s => s.Rate >= 4.0).
                    Take(10).ToListAsync(),

                AvailableItems = await _context.Item.Where(i => i.Offers.Count > 0).ToListAsync(),

                Advertisments = await _context.Advertisment.ToListAsync()

            };

            return View(model);
        }

        public async Task<IActionResult> Search(String searchQuery)
        {
            if (searchQuery == null)
                searchQuery = String.Empty;

            SearchViewModel model = new SearchViewModel
            {
                items = await _context.Item.Where(i => i.ItemName.ToLower().Contains(searchQuery.ToLower())).ToListAsync(),
                stores = await _context.Store.Where(i => i.StoreName.ToLower().Contains(searchQuery.ToLower())).ToListAsync(),
                categories = await _context.Category.Where(i => i.CategoryName.ToLower().Contains(searchQuery.ToLower())).ToListAsync(),
                searchQuery = searchQuery
            };

            return View(model);
        }

        public IActionResult AdminDashboard()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
