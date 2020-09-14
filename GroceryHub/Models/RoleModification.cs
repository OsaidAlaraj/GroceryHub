using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryHub.Models
{
    public class RoleModification
    {
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string[] AddIds { get; set; }
        public string[] DeleteIds { get; set; }
    }
}
