using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryHub.Models
{
    public class Item
    {
        [Key]
        public int ItemID { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Item Name")]
        public String ItemName { get; set; }

        [Display(Name = "Item Image")]
        public Byte[] Image { get; set; }

        public Category Category { get; set; }
        public int CategoryID { get; set; }

        public List<Offer> Offers { get; set; }
    }
}
