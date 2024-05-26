using WebBanHang.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace DoAnCuoiky.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IProductRespository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ApplicationDbContext _context;
        public CategoryController(IProductRespository productRepository, ICategoryRepository categoryRepository, ApplicationDbContext context)

        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _context = context;
        }
        public async Task<IActionResult> Index(string searchString, int? page, int? pageSize)
        {
            ViewData["CurrentFilter"] = searchString;

            var categoryQuery = _context.Categories.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                categoryQuery = categoryQuery.Where(n => n.Name.ToLower().Contains(searchString.ToLower()));
            }

            int defaultPageSize = pageSize ?? 10; // Default page size is 10 if not provided
            int pageNumber = page ?? 1; // Default page number is 1 if not provided

            var pagedCategory= await categoryQuery.ToPagedListAsync(pageNumber, defaultPageSize);

            ViewBag.PageSize = new SelectList(new List<int> { 10, 20, 50 }, defaultPageSize);
            ViewBag.CurrentPageSize = defaultPageSize; // Update the value of ViewBag.CurrentPageSize

            return View(pagedCategory);
        }
        public async Task<IActionResult> Add()
        {


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Category category)
        {

            
            
                await _categoryRepository.AddAsync(category);
                return RedirectToAction(nameof(Index));


            
          

            
        }
        public async Task<IActionResult> Display(int id)
        {
            var categorys = await _categoryRepository.GetByIdAsync(id);
            if (categorys == null)
            {
                return NotFound();
            }
            return View(categorys);
        }
        public async Task<IActionResult> Update(int id)
        {
            var categories = await _categoryRepository.GetByIdAsync(id);
            if (categories == null)
            {
                return NotFound();
            }





            return View(categories);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Category category)

        {


            if (id != category.Id)
            {
                return NotFound();
            }
            
                var existingProduct = await

                _categoryRepository.GetByIdAsync(id);

                existingProduct.Name = category.Name;

                await _categoryRepository.UpdateAsync(existingProduct);
                return RedirectToAction(nameof(Index));
            


            return View(category);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        // Xử lý xóa sản phẩm
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryRepository.DeleteAsync(id);

            return Ok();
        }
    }
}
