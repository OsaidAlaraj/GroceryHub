using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryHub.ViewModels
{
    public class FlyerReaderViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        public List<IFormFile> photos { get; set; }
        public String Data { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        public int StoreID { get; set; }
    }
}
