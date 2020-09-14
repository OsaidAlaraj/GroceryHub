using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryHub.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Category Name")]
        public String CategoryName { get; set; }
        public Byte[] Icon { get; set; }

        public List<Item> Items { get; set; }
    }
}
