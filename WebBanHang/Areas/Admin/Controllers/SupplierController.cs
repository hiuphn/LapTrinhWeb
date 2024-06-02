using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;
using X.PagedList;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class SupplierController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly ISupplierRespository _supplierRespository;


        public SupplierController(ApplicationDbContext context, ISupplierRespository supplierRespository)
        {
            _context = context;
            _supplierRespository = supplierRespository;
        }

        // GET: Admin/NhaCungCaps
        public async Task<IActionResult> Index(string searchString, int? page, int? pageSize)
        {
            ViewData["CurrentFilter"] = searchString;

            var supplierQuery = _context.Suppliers.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                supplierQuery = supplierQuery.Where(n => n.CompanyName.ToLower().Contains(searchString.ToLower()));
            }

            int defaultPageSize = pageSize ?? 10; // Default page size is 10 if not provided
            int pageNumber = page ?? 1; // Default page number is 1 if not provided

            var pagedSupplier = await supplierQuery.ToPagedListAsync(pageNumber, defaultPageSize);

            ViewBag.PageSize = new SelectList(new List<int> { 10, 20, 50 }, defaultPageSize);
            ViewBag.CurrentPageSize = defaultPageSize; // Update the value of ViewBag.CurrentPageSize

            return View(pagedSupplier);
        }

        // GET: Admin/NhaCungCaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaCungCap = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierID == id);
            if (nhaCungCap == null)
            {
                return NotFound();
            }

            return View(nhaCungCap);
        }

        // GET: Admin/NhaCungCaps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/NhaCungCaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(Supplier nhacungcap, IFormFile logo)
        {
            if (ModelState.IsValid)
            {
                if (logo != null)
                {
                    // Lưu hình ảnh đại diện
                    nhacungcap.Logo = await SaveImage(logo);
                }


                 await _supplierRespository.AddAsync(nhacungcap);

                return RedirectToAction(nameof(Index));
            }



            return View(nhacungcap);
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
        // GET: Admin/NhaCungCaps/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var nhacungcap = await _supplierRespository.GetByIdAsync(id);
            if (nhacungcap == null)
            {
                return NotFound();
            }


            return View(nhacungcap);
        }
        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Supplier nhacungcap, IFormFile logo)

        {
            ModelState.Remove("Logo"); // Loại bỏ xác thực ModelState cho ImageUrl

            if (id != nhacungcap.SupplierID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingProduct = await _supplierRespository.GetByIdAsync(id); // Giả định có phương thức GetByIdAsync
                                                                                  // Giữ nguyên thông tin hình ảnh nếu không có hình mới được tải lên

                if (logo == null)
                {
                    nhacungcap.Logo = existingProduct.Logo;
                }
                else
                {
                    // Lưu hình ảnh mới
                    nhacungcap.Logo = await SaveImage(logo);
                }
                // Cập nhật các thông tin khác của sản phẩm
                existingProduct.CompanyName = nhacungcap.CompanyName;
                existingProduct.Logo= nhacungcap.Logo;
                existingProduct.Email = nhacungcap.Email;
                existingProduct.SuplierPhone = nhacungcap.SuplierPhone;
                existingProduct.SupplierAddress = nhacungcap.SupplierAddress;
                existingProduct.Description = nhacungcap.Description;
                existingProduct.Logo= nhacungcap.Logo;
                await _supplierRespository.UpdateAsync(existingProduct);
                return RedirectToAction(nameof(Index));
            }

            return View(nhacungcap);
        }

        // GET: Admin/NhaCungCaps/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var nhacungcap = await _supplierRespository.GetByIdAsync(id);
            if (nhacungcap == null)
            {
                return NotFound();
            }
            return View(nhacungcap);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _supplierRespository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private bool NhaCungCapExists(int id)
        {
            return _context.Suppliers.Any(e => e.SupplierID == id);
        }
    }
}
