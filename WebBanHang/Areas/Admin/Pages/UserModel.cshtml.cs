using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebBanHang.Models; // Thay thế "YourAppName" bằng namespace của ứng dụng của bạn
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Azure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Printing;


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

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedRole { get; set; }
        [BindProperty(SupportsGet = true)]
        public string AccountStatus { get; set; }

        public async Task OnGetAsync()
        {
            var users = from u in _userManager.Users
                        select u;

            if (!string.IsNullOrEmpty(SearchString))
            {
                users = users.Where(u => u.FullName.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(SelectedRole))
            {
                users = users.Where(u => u.ChucVu == SelectedRole);
            }
            if (!string.IsNullOrEmpty(AccountStatus))
            {
                users = users.Where(u => u.IsLocked.ToString() == AccountStatus);
            }

            Users = await users.ToListAsync();
           
          
        }
    }
}
