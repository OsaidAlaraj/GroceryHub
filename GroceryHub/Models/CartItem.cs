using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryHub.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemID { get; set; }

        public Offer Offer { get; set; }
        public int OfferID { get; set; }


        public Cart Cart { get; set; }
        public int CartID { get; set; }
    }
}
