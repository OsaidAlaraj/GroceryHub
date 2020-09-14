using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryHub.Models
{
    public class Cart
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartID { get; set; }

        //Cart Item props
        public List<CartItem> CartItems { get; set; }

        //User props
        public String AppUserID { get; set; }
        public AppUser AppUser { get; set; }
    }
}
