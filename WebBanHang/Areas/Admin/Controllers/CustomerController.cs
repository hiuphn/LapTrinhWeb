using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName); // Thay
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return "/images/" + image.FileName; // Trả về đường dẫn tương đối
        }
        // GET: Admin/KhachHangs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        // GET: Admin/KhachHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // GET: Admin/KhachHangs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/KhachHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,UserId,CustomerName,Sex,BirthDay,CustomerAddress,CustomerPhone,Email,CustomerImage")] Customer khachHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khachHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: Admin/KhachHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.Customers.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, Customer nhanVien, string userId, IFormFile logo)
        {
            ModelState.Remove("CustomerImage");
            if (id != nhanVien.CustomerID)
            {
                return NotFound();
            }

            // Lấy thông tin nhân viên từ cơ sở dữ liệu
            var existingNhanVien = await _context.Customers.FindAsync(nhanVien.CustomerID);

            if (existingNhanVien == null)
            {
                return NotFound();
            }
            if (logo == null)
            {
                nhanVien.CustomerImage = existingNhanVien.CustomerImage;
            }
            else
            {
                // Lưu hình ảnh mới
                nhanVien.CustomerImage = await SaveImage(logo);
            }

            // Cập nhật thông tin của nhân viên từ dữ liệu nhập vào
            existingNhanVien.CustomerName = nhanVien.CustomerName;
            existingNhanVien.Sex = nhanVien.Sex;
            existingNhanVien.BirthDay = nhanVien.BirthDay;
            existingNhanVien.CustomerAddress = nhanVien.CustomerAddress;
            existingNhanVien.CustomerImage = nhanVien.CustomerImage;
            existingNhanVien.CustomerPhone = nhanVien.CustomerPhone;
            existingNhanVien.Email = nhanVien.Email;

            // Lưu các thay đổi vào cơ sở dữ liệu
            _context.Update(existingNhanVien);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));


            return View(nhanVien);
        }
        

        // GET: Admin/KhachHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // POST: Admin/KhachHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khachHang = await _context.Customers.FindAsync(id);
            if (khachHang != null)
            {
                _context.Customers.Remove(khachHang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }
    }
}
