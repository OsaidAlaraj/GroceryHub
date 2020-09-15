using GroceryHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryHub.ViewModels
{
    public class SearchViewModel
    {
        public List<Item> items { get; set; }
        public List<Store> stores { get; set; }
        public List<Category> categories { get; set; }
        public String searchQuery { get; set; }
    }
}
