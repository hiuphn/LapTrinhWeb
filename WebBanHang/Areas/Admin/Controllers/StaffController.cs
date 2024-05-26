using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;
using X.PagedList;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class StaffController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;


        public StaffController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin/NhanViens
        public async Task<IActionResult> Index(string searchString, int? page, int? pageSize)
        {
            ViewData["CurrentFilter"] = searchString;

            var staffsQuery = _context.Staffs.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                staffsQuery = staffsQuery.Where(n => n.StaffName.ToString().Contains(searchString.ToLower()));
            }

            int defaultPageSize = pageSize ?? 10; // Default page size is 10 if not provided
            int pageNumber = page ?? 1; // Default page number is 1 if not provided

            var pagedStaffs = await staffsQuery.ToPagedListAsync(pageNumber, defaultPageSize);

            ViewBag.PageSize = new SelectList(new List<int> { 10, 20, 50 }, defaultPageSize);
            ViewBag.CurrentPageSize = defaultPageSize; // Update the value of ViewBag.CurrentPageSize

            return View(pagedStaffs);
        }

        // GET: Admin/NhanViens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.Staffs
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // GET: Admin/NhanViens/Create
        public IActionResult Create()
        {
            return View();
        }
        private async Task<string> SaveImage(IFormFile image)
        {
            if (image == null)
            {
                // Sử dụng hình ảnh mặc định nếu không có ảnh tải lên
                return "/images/anonymous.png";
            }

            var savePath = Path.Combine("wwwroot/images", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return "/images/" + image.FileName; // Trả về đường dẫn tương đối
        }
        // POST: Admin/NhanViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StaffViewModel staff, IFormFile AvatarPath)
        {
            
         
                var imagePath = await SaveImage(AvatarPath);
                var user = new ApplicationUser {
                    UserName = staff.Email,
                    FullName = staff.StaffName,
                    Email = staff.Email,
                    Age = staff.BirthDay,
                    Sex = staff.Sex,
                    PhoneNumber = staff.StaffPhone,
                    AvatarPath = imagePath,
                    Address = staff.Address};
                var result = await _userManager.CreateAsync(user, staff.Password);
                if (result.Succeeded)
                {
                    var role = await _roleManager.FindByNameAsync(staff.Position);
                    if (role == null)
                    {
                        role = new IdentityRole(staff.Position);
                        await _roleManager.CreateAsync(role);
                    }

                    await _userManager.AddToRoleAsync(user, role.Name);

                    var Staff = new Staff
                    {
                        StaffName = staff.StaffName,
                        Sex = staff.Sex,
                        Position = staff.Position,
                        BirthDay = staff.BirthDay,
                        Address = staff.Address,
                        StaffPhone = staff.StaffPhone,
                        Email = staff.Email,
                        UserId = user.Id,
                        RoleId = role.Id,
                        AvatarPath=imagePath
                    };

                    _context.Add(Staff);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                AddErrors(result);
            
            return View(staff);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        // GET: Admin/NhanViens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.Staffs.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }

        // POST: Admin/NhanViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Staff nhanVien, string userId, IFormFile logo)
        {
            ModelState.Remove("AvatarPath");
            if (id != nhanVien.StaffId)
            {
                return NotFound();
            }

            // Lấy thông tin nhân viên từ cơ sở dữ liệu
            var existingNhanVien = await _context.Staffs.FindAsync(nhanVien.StaffId);

            if (existingNhanVien == null)
            {
                return NotFound();
            }
            if (logo == null)
            {
                nhanVien.AvatarPath = existingNhanVien.AvatarPath;
            }
            else
            {
                // Lưu hình ảnh mới
                nhanVien.AvatarPath = await SaveImage(logo);
            }

            // Cập nhật thông tin của nhân viên từ dữ liệu nhập vào
            existingNhanVien.StaffName = nhanVien.StaffName;
            existingNhanVien.Sex = nhanVien.Sex;
            existingNhanVien.BirthDay = nhanVien.BirthDay;
            existingNhanVien.Address = nhanVien.Address;
            existingNhanVien.AvatarPath = nhanVien.AvatarPath;
            existingNhanVien.StaffPhone = nhanVien.StaffPhone;
            existingNhanVien.Email = nhanVien.Email;


          
            if (nhanVien.Position != existingNhanVien.Position)
            {
                // Lấy RoleId mới từ cơ sở dữ liệu dựa trên tên vai trò mới
                var newRole = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == nhanVien.Position);

                if (newRole != null)
                {
                    // Cập nhật RoleId của nhân viên
                    existingNhanVien.RoleId = newRole.Id;
                    existingNhanVien.Position = newRole.Name;
                    var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == existingNhanVien.UserId);
                    if (userRole != null)
                    {
                        _context.UserRoles.Remove(userRole);

                        // Tạo bản ghi mới với roleId mới
                        var newUserRole = new IdentityUserRole<string> { UserId = existingNhanVien.UserId, RoleId = newRole.Id };

                        // Thêm bản ghi mới vào bảng AspNetUserRoles
                        _context.UserRoles.Add(newUserRole);

                        // Lưu thay đổi vào cơ sở dữ liệu
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    // Xử lý trường hợp vai trò mới không tồn tại
                    ModelState.AddModelError(string.Empty, "Vai trò không hợp lệ");
                    return View(nhanVien);
                }
            }

            // Lưu các thay đổi vào cơ sở dữ liệu
            _context.Update(existingNhanVien);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));


            return View(nhanVien);
        }

        // GET: Admin/NhanViens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.Staffs
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // POST: Admin/NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhanVien = await _context.Staffs.FindAsync(id);
            if (nhanVien != null)
            {
                _context.Staffs.Remove(nhanVien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhanVienExists(int id)
        {
            return _context.Staffs.Any(e => e.StaffId == id);
        }
    }
}

