using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryHub.Models
{
    public enum Governorates
    {
        [Display(Name = "Irbid")]
        Irbid,
        [Display(Name = "Ajloun")]
        Ajloun, 
        [Display(Name = "Jerash")]
        Jerash,
        [Display(Name = "Mafraq")]
        Mafraq,
        [Display(Name = "Balqa")]
        Balqa,
        [Display(Name = "Amman")]
        Amman,
        [Display(Name = "Zarqa")]
        Zarqa,
        [Display(Name = "Madaba")]
        Madaba,
        [Display(Name = "Karak")]
        Karak,
        [Display(Name = "Tafilah")]
        Tafilah,
        [Display(Name = "Ma'an")]
        Maan,
        [Display(Name = "Aqaba")]
        Aqaba
    }
}
