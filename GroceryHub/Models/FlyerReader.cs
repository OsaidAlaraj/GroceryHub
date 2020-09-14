using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryHub.Models
{
    public class FlyerReader
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public List<ByteWraper> Img { get; set; }
        public String Data { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        public virtual Store Store { get; set; }
        public int StoreID { get; set; }
        [ForeignKey("FlyerOffer")]
        public List<FlyerOffer> flyerOffers { get; set; }
    }
}
