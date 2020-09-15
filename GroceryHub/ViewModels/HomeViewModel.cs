using GroceryHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryHub.ViewModels
{
    public class HomeViewModel
    {
        public List<Offer> HotOffers { get; set; }
        public List<Offer> LastAddedOffers { get; set; }
        public List<Store> TopRatedStores { get; set; }
        public List<Advertisment> Advertisments { get; set; }
        public List<Item> AvailableItems { get; set; }

    }
}
