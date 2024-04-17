using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebBanHang.Models; // Thay thế "YourAppName" bằng namespace của ứng dụng của bạn
using System.Collections.Generic;
using System.Threading.Tasks;


namespace WebBanHang.Areas.Admin.Pages
{

    [Authorize(Roles = "Admin")]
    public class UserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IList<ApplicationUser> Users { get; set; }

        public async Task OnGetAsync()
        {
            Users = _userManager.Users.ToList();
        }
    }
}
