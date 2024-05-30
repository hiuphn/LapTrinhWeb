using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Encodings.Web;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Pages 
{
    public class banedModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender1 _emailSender;

        public banedModel(UserManager<ApplicationUser> userManager, IEmailSender1 emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }


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
            Phone = user.PhoneNumber;
            Address = user.Address;

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
                var callbackUrl = Url.Page(
                "Identity/Account/Login",
                pageHandler: null,
                values: null,
                protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Your account has been locked",
                    $"Your account has been locked. Please <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>login</a> to check.");

            }
            else
            {
                return BadRequest();    
            }


            return Redirect("/Admin/UserModel");
        }   

        
    }
}
