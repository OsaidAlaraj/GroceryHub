using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GroceryHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace GroceryHub.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;


        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel : AppUser
        {
            [Display(Name = "First Name", Prompt = "Your first name")]
            public String FirstName { get; set; }
            [Display(Name = "Last Name", Prompt = "Your last name")]
            public String LastName { get; set; }
            [Display(Name = "Date of Birth", Prompt = "Your date of birth")]
            [DataType(DataType.Date)]
            public Governorates Address { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }
        }

        private async Task LoadAsync(AppUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            AppUser current = await _userManager.FindByNameAsync(userName);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = current.PhoneNumber,
                FirstName = current.FirstName,
                LastName = current.LastName,
                Address = current.Address,
                Email = current.Email,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }


        public async Task<IActionResult> OnPostUpdateUserInfo(InputModel Input)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (ModelState.IsValid)
            {
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.Address = Input.Address;
                user.PhoneNumber = Input.PhoneNumber;

                IdentityResult result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    StatusMessage = "Your profile has been updated";
                    return RedirectToPage();
                }
                var userId = await _userManager.GetUserIdAsync(user);
                StatusMessage = "Unexpected error occurred";
                throw new InvalidOperationException($"Unexpected error occurred setting Info for user with ID '{userId}'.");
            }

            else
            {
                await LoadAsync(user);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostChangePassword()
        {

            var propertyToValidate = ModelState["Input.OldPassword"];
            if (propertyToValidate == null || (propertyToValidate != null && propertyToValidate.Errors.Any()))
            {
                var anotherpropertyToValidate = ModelState["Input.NewPassword"];
                if (anotherpropertyToValidate == null || (anotherpropertyToValidate != null && anotherpropertyToValidate.Errors.Any()))
                {
                    return Page();
                }
            }




            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await LoadAsync(user);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your password has been changed.";
            await LoadAsync(user);
            return RedirectToPage();
        }

    }
}
