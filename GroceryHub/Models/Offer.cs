using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryHub.Models
{
    public class Offer
    {
        [Key]
        public int OfferID { get; set; }
        public Double Unit { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Currency)]
        public Double Price { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        [Range(0, 5)]
        [Display(Name = "Level")]
        public int Level { get; set; }

        [Display(Name = "Description")]
        public String OfferDescription { get; set; }

        public bool IsValid { get; set; } = true;

        public Store Store { get; set; }
        public int StoreID { get; set; }

        public Item Item { get; set; }
        public int ItemID { get; set; }

        public List<CartItem> cartItems { get; set; } = new List<CartItem>();
    }
}
