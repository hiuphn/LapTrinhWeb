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

    public class ProductController : Controller
    {
        private readonly IProductRespository _productRespository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubcategory _subcategory;
        private readonly ISupplierRespository _supplierRespository;
        private readonly ApplicationDbContext _context;

        public ProductController(IProductRespository productRespository, ICategoryRepository categoryRepository, ISubcategory subcategory, ISupplierRespository supplierRespository, ApplicationDbContext context)
        {
            _productRespository = productRespository;
            _categoryRepository = categoryRepository;
            _subcategory = subcategory;
            _supplierRespository = supplierRespository;
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, int? page, int? pageSize)
        {
            ViewData["CurrentFilter"] = searchString;

            var productQuery = _context.Products.Include(p=>p.Category)
                                                .Include(c=>c.Supplier)
                                                .Include(d=>d.Subcategory).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                productQuery = productQuery.Where(n => n.Name.ToLower().Contains(searchString.ToLower()));
            }

            int defaultPageSize = pageSize ?? 10; // Default page size is 10 if not provided
            int pageNumber = page ?? 1; // Default page number is 1 if not provided

            var pagedProduct = await productQuery.ToPagedListAsync(pageNumber, defaultPageSize);

            ViewBag.PageSize = new SelectList(new List<int> { 10, 20, 50 }, defaultPageSize);
            ViewBag.CurrentPageSize = defaultPageSize; // Update the value of ViewBag.CurrentPageSize

            return View(pagedProduct);
        }
     
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.categories = new SelectList(categories, "Id", "Name");
            ViewBag.Subcategory = new SelectList(Enumerable.Empty<Subcategory>(), "Id", "Name");
            var Supplier = await _supplierRespository.GetAllAsync();
            ViewBag.Supplier = new SelectList(Supplier, "SupplierID", "CompanyName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> images)
        {
            //if (ModelState.IsValid)
            //{
                if (imageUrl != null)
                {
                    // Lưu hình ảnh đại diện
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                if (images != null)
                {
                    product.Images = new List<ProductImage>();
                    foreach (var item in images)
                    {
                        ProductImage image = new ProductImage()
                        {
                            ProductId = product.Id,
                            Url = await SaveImage(item)
                        };
                        product.Images.Add(image);
                    }
                }

                await _productRespository.AddAsync(product);

                return RedirectToAction(nameof(Index));
            //}
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            var Subcategory = await _subcategory.GetAllAsync();
            ViewBag.Subcategory = new SelectList(Subcategory, "Id", "Name");
            var Supplier = await _supplierRespository.GetAllAsync();
            ViewBag.Supplier = new SelectList(Supplier, "SupplierID", "CompanyName");

            return View(product);
        }

        public async Task<JsonResult> GetSubcategories(int categoryId)
        {
            var subcategories = await _subcategory.GetSubcategoriesByCategoryIdAsync(categoryId);
            return Json(new SelectList(subcategories, "Id", "Name"));
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
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRespository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();

            }
           
            var productImages = await _productRespository.GetImagesByProductIdAsync(id);
            ViewBag.ProductImages = productImages;
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            var Subcategories = await _subcategory.GetAllAsync();
            ViewBag.Subcategories = new SelectList(Subcategories, "Id", "Name", product.SubcategoryId);
            var Supplier = await _supplierRespository.GetAllAsync();
            ViewBag.Supplier = new SelectList(Supplier, "Id", "Name", product.SupplierID);
            return View(product);   
        }

        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRespository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.categories = new SelectList(categories, "Id", "Name");
            ViewBag.Subcategory = new SelectList(Enumerable.Empty<Subcategory>(), "Id", "Name");
            var Supplier = await _supplierRespository.GetAllAsync();
            ViewBag.Supplier = new SelectList(Supplier, "SupplierID", "CompanyName");
            var productImages = await _productRespository.GetImagesByProductIdAsync(id);
            ViewBag.ProductImages = productImages;
            return View(product);
        }
        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, IFormFile imageUrl, List<IFormFile> images)

        {
            ModelState.Remove("ImageUrl"); // Loại bỏ xác thực ModelState cho ImageUrl
            ModelState.Remove("Images");
            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingProduct = await _productRespository.GetByIdAsync(id); // Giả định có phương thức GetByIdAsync
                                                                                  // Giữ nguyên thông tin hình ảnh nếu không có hình mới được tải lên

                if (imageUrl == null)
                {
                    product.ImageUrl = existingProduct.ImageUrl;
                }
                else
                {
                    // Lưu hình ảnh mới
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                if (images != null)
                {
                    product.Images = new List<ProductImage>();
                    foreach (var item in images)
                    {
                        ProductImage image = new ProductImage()
                        {
                            ProductId = product.Id,
                            Url = await SaveImage(item)
                        };
                        product.Images.Add(image);
                    }
                }
                else
                {
                    product.Images=existingProduct.Images;
                }
                // Cập nhật các thông tin khác của sản phẩm
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.SubcategoryId= product.SubcategoryId;
                existingProduct.SupplierID = product.SupplierID;
                existingProduct.ImageUrl = product.ImageUrl;
                existingProduct.Images = product.Images;
                await _productRespository.UpdateAsync(existingProduct);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.categories = new SelectList(categories, "Id", "Name");
            ViewBag.Subcategory = new SelectList(Enumerable.Empty<Subcategory>(), "Id", "Name");
            var Supplier = await _supplierRespository.GetAllAsync();
            ViewBag.Supplier = new SelectList(Supplier, "SupplierID", "CompanyName");
            var productImages = await _productRespository.GetImagesByProductIdAsync(id);
            ViewBag.ProductImages = productImages;
            return View(product);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRespository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRespository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productRespository.DeleteAsync(id);

            return Ok();
        }
    }


}
    