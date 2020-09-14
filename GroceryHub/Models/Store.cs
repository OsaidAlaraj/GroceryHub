using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryHub.Models
{
    public class Store
    {
        [Key]
        public int StoreID { get; set; }
        [Display(Name = "Store Name")]
        [Required(ErrorMessage = "This field is required")]
        public String StoreName { get; set; }
        [Display(Name = "Location")]
        [Required(ErrorMessage = "This field is required")]
        public Governorates Location { get; set; }
        [Display(Name = "Store Rate")]
        [Range(0, 5.0)]
        public Double Rate { get; set; }
        [Display(Name = "Store Website")]
        [DataType(DataType.Url)]
        public String Website { get; set; }
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public String PhoneNumber { get; set; }
        [Display(Name = "Open Hour")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "0:hh\\:mm")]
        public DateTime OpenHour { get; set; }
        [Display(Name = "Close Hour")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "0:hh\\:mm")]
        public DateTime CloseHour { get; set; }
        [Display(Name = "Market Type")]
        public MarketType Type { get; set; }
        [Display(Name = "Store Logo")]
        public Byte[] Image { get; set; }

        public List<Offer> Offers { get; set; }
    }
}
