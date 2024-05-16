using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;

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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Staffs.ToListAsync());
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

        // POST: Admin/NhanViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,StaffName,Sex,Position,BirthDay,Address,StaffPhone,Email,UserId")] Staff nhanVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhanVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhanVien);
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
        public async Task<IActionResult> Edit(int id, [Bind("StaffId,StaffName,Sex,Position,BirthDay,Address,StaffPhone,Email,UserId")] Staff nhanVien, string userId)
        {
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

            // Cập nhật thông tin của nhân viên từ dữ liệu nhập vào
            existingNhanVien.StaffName = nhanVien.StaffName;
            existingNhanVien.Sex = nhanVien.Sex;
            existingNhanVien.BirthDay = nhanVien.BirthDay;
            existingNhanVien.Address = nhanVien.Address;
            existingNhanVien.StaffPhone = nhanVien.StaffPhone;
            existingNhanVien.Email = nhanVien.Email;


            //// Tìm RoleId mới của admin
            //var adminRole = await _roleManager.FindByNameAsync("Admin");
            //var adminRoleId = adminRole.Id;

            //// Tìm và cập nhật bản ghi trong bảng AspNetUserRoles
            //var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == Id);
            //if (userRole != null)
            //{
            //    userRole.RoleId = adminRoleId;
            //    await _context.SaveChangesAsync();
            //}
            // Kiểm tra xem có thay đổi vai trò không
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

