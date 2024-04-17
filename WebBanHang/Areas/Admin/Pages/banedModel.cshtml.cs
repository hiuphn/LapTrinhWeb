using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Pages 
{
    public class banedModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public banedModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public string UserName { get; set; }
        public string Email { get; set; }

        public IActionResult OnGet(string id)
        {
            // Lấy thông tin tài khoản từ cơ sở dữ liệu dựa trên id
            var user = _userManager.FindByIdAsync(id).Result;

            if (user == null)
            {
                return NotFound();
            }

            UserName = user.UserName;
            Email = user.Email;

            return Page();
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(string id)
        {
            // Xóa tài khoản từ cơ sở dữ liệu dựa trên id
            var user = _userManager.FindByIdAsync(id).Result;

            if (user == null)
            {
                return NotFound();
            }

            if(user.IsLocked == false)
            {
                user.IsLocked = true;
                await _userManager.UpdateAsync(user);

                // Xử lý sau khi tài khoản đã được khóa

            }
            else
            {
                return BadRequest();    
            }

            
            return RedirectToAction("Index");
        }
    }
}
