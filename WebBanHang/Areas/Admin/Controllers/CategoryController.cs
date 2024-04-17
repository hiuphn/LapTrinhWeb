using WebBanHang.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DoAnCuoiky.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IProductRespository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(IProductRespository productRepository, ICategoryRepository categoryRepository)

        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return View(categories);
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
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
