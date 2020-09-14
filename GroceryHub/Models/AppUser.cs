using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryHub.Models
{
    public class AppUser : IdentityUser
    {
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "First Name", Prompt = "Your first name")]
        public String FirstName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Last Name", Prompt = "Your last name")]
        public String LastName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Date of Birth", Prompt = "Your date of birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public Governorates Address { get; set; }

        public int CartID { get; set; }
        public Cart Cart { get; set; }
    }
}