using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IProductRespository _productRespository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubcategory _subcategory;

        public ProductController(IProductRespository productRespository, ICategoryRepository categoryRepository, ISubcategory subcategory)
        {
            _productRespository = productRespository;
            _categoryRepository = categoryRepository;
            _subcategory = subcategory; 
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRespository.GetAllAsync();
           



            return View(products);
        }
        public async Task<IActionResult> Add(int cata )
        {
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            /*var categories = await _categoryRepository.GetAllAsync();
            ViewBag.categories = new SelectList(categories, "Id", "Name");*/
            /*var subcategories = await _subcategory.GetSubcategoriesByCategoryIdAsync(cata);
            ViewBag.Subcategory = new SelectList(subcategories, "Id", "Name");*/
            ViewBag.Subcategory = new SelectList(Enumerable.Empty<Subcategory>(), "Id", "Name");
            



            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> images, int categoryId)
        {
            if (ModelState.IsValid)
            {
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
            }
            /*var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            var subcategories = await _subcategory.GetSubcategoriesByCategoryIdAsync(categoryId);
            ViewBag.Subcategory = new SelectList(subcategories, "Id", "Name");*/
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", product.CategoryId);
            ViewBag.Subcategory = new SelectList(await _subcategory.GetSubcategoriesByCategoryIdAsync(product.CategoryId), "Id", "Name", product.SubcategoryId);


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
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);

            return View(product);
        }
        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, IFormFile imageUrl)

        {
            ModelState.Remove("ImageUrl"); // Loại bỏ xác thực ModelState cho ImageUrl

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
                // Cập nhật các thông tin khác của sản phẩm
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = product.ImageUrl;
                await _productRespository.UpdateAsync(existingProduct);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
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
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRespository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
      


    }
}
    