﻿using Microsoft.AspNetCore.Mvc;
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
                pageSize = 9;
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
            var savePath = Path.Combine("wwwroot/images", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return "/images/" + image.FileName;
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

        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, IFormFile imageUrl)
        {
            ModelState.Remove("ImageUrl");

            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingProduct = await _productRespository.GetByIdAsync(id);

                if (imageUrl == null)
                {
                    product.ImageUrl = existingProduct.ImageUrl;
                }
                else
                {
                    product.ImageUrl = await SaveImage(imageUrl);
                }

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

        public IActionResult Search(string query, int page = 1)
        {
            int pageSize = 10;
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

        public async Task<IActionResult> DisplayProducts(int categoryId, int page = 1)
        {
            int pageSize = 9; // Số sản phẩm trên mỗi trang
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            var products = await _productRespository.GetByCategoryIdAsync(categoryId);
            var pagedProducts = products.ToPagedList(page, pageSize);

            var viewModel = new DisplayProductsViewModel
            {
                Category = category,
                Products = pagedProducts
            };
            return View(viewModel);
        }

        public async Task<IActionResult> DisplayProductsDetail(int subcategoryId)
        {
            var subcategory = await _subcategory.GetByIdAsync(subcategoryId);
            var products = await _productRespository.GetBySubCategoryIdAsync(subcategoryId);

            var viewModel = new DisplayProductdetail
            {
                subcategory = subcategory,
                Products = products
            };
            return View(viewModel);
        }
    }
}
