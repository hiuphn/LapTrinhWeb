using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Encodings.Web;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Pages
{
    public class UnlockModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender1 _emailSender;

        public UnlockModel(UserManager<ApplicationUser> userManager, IEmailSender1 emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
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

                
                var callbackUrl = Url.Page(
                "Identity/Account/Login",
                pageHandler: null,
                values: null,
                protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Khóa tài khoản",
                    $"Tài khoản của bạn đã đẫ được mở khóa <br><br>. Vui lòng <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Đăng nhập</a> để kiểm tra.");
            }
            else
            {
                return BadRequest();
            }

            return Redirect("/Admin/UserModel");
        }
    }
}
