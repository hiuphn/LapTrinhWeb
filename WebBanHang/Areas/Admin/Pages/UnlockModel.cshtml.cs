using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Pages
{
    public class UnlockModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UnlockModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            UserName = user.UserName;
            Email = user.Email;
            Phone = user.PhoneNumber;
            Address = user.Address;

            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.IsLocked)
            {
                user.IsLocked = false;
                await _userManager.UpdateAsync(user);
            }
            else
            {
                return BadRequest();
            }

            return Redirect("/Admin/UserModel");
        }
    }
}
