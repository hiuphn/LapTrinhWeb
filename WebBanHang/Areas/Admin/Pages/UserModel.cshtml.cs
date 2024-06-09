using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebBanHang.Models; 
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Azure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Printing;
using X.PagedList;


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

        public IPagedList<ApplicationUser> Users { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedRole { get; set; }
        [BindProperty(SupportsGet = true)]
        public string AccountStatus { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 3;

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

            Users = await users.ToPagedListAsync(PageNumber, PageSize);


        }
    }
}
