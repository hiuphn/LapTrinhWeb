using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;
using X.PagedList;
namespace WebBanHang.Controllers
{
    public class Product1Controller : Controller
    {

        private readonly IProductRespository _productRespository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubcategory _subcategory;
        private readonly ApplicationDbContext _context;
        public Product1Controller(IProductRespository productRespository, ICategoryRepository categoryRepository, ISubcategory subcategory, ApplicationDbContext context)
        {
            _productRespository = productRespository;
            _categoryRepository = categoryRepository;
            _subcategory = subcategory;
            _context = context;
        }

        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            if(page == null)
            {
                page = 1;
            }
            if(pageSize == null)
            {
                pageSize = 2;
            }
            ViewBag.nameCategory = await _categoryRepository.GetAllAsync();
            var products = _context.Products.ToList();
            //var products = await _productRespository.GetAllAsync();
            /*List<Product> motors = products.Where(p => p.Category.Name == "Quà tặng").Take(4).ToList();
            ViewBag.Motors = motors;*/
            return View(products.ToPagedList((int)page, (int) pageSize));
        }

        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.categories = new SelectList(categories, "Id", "Name");
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> images)
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
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            
            return View(product);
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
            var products = await _productRespository.GetAllAsync();
            ViewBag.productList_8 = products.Take(8);
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
        public IActionResult Search(string query)
        {

            using (var context = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()))
            {
                List<Product> searchResults = context.Products
                    .Include(p => p.Category)
                    .Where(p => p.Name.Contains(query))
                    .ToList();

                if (searchResults.Count == 0)
                {

                    return View("NoResults");
                }
                return View(searchResults);
            }
        }
        /*public IActionResult Search(string query, string imagePath)
        {
            
            using (var context = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()))
            {
                List<Product> searchResults = context.Products
                    .Include(p => p.Category)
                    .Where(p => p.Name.Contains(query))
                    .ToList();

                if (searchResults.Count == 0)
                {
                    imagePath = "/img/404NotFound.jpg";
                    return View("NoResults", imagePath);
                }
                return View(searchResults);
            }
        }
        public IActionResult SearchAction(string query)
        {
            string imagePath = "/images/no_results.jpg"; // Đường dẫn đến hình ảnh khi không tìm thấy kết quả
            return Search(query, imagePath);
        }*/
        public async Task<IActionResult> DisplayProducts(int categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            var products = await _productRespository.GetByCategoryIdAsync(categoryId);
          
            // Truy vấn các sản phẩm theo danh mục categoryId
            var viewModel = new DisplayProductsViewModel
            {
                Category = category,
                Products = products
            };
            return View( viewModel);
        }
        public async Task<IActionResult> DisplayProductsDetail(int subcategoryId)
        {
            var subcategory = await _subcategory.GetByIdAsync(subcategoryId);
            var products = await _productRespository.GetBySubCategoryIdAsync(subcategoryId);

            // Truy vấn các sản phẩm theo danh mục categoryId
            var viewModel = new DisplayProductdetail
            {
                subcategory = subcategory,
                Products = products

            };
            return View(viewModel);
        }
        
    }
    
}

