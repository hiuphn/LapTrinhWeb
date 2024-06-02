//using Microsoft.AspNetCore.Mvc;
//using WebBanHang.Models;

//namespace WebBanHang.Areas.Admin.Controllers
//{
//    public class SubcategoryController : Controller
//    {


//        private readonly IProductRespository _productRepository;
//        private readonly ICategoryRepository _categoryRepository;
//        private readonly ISubcategory _subcategory;

//        public SubcategoryController(IProductRespository productRepository, ICategoryRepository categoryRepository, ISubcategory subcategory)
//        {
//            _productRepository = productRepository;
//            _categoryRepository = categoryRepository;
//            _subcategory = subcategory;
//        }


//    //    private List<Category> _categories = new List<Category>
//    //{
//    //    new Category
//    //    {
//    //        Id = 1,
//    //        Name = "Ô tô",
//    //        Subcategories = new List<Subcategory>
//    //        {
//    //            new Subcategory { Id = 1, Name = "Xe hơi" },
//    //            new Subcategory { Id = 2, Name = "Xe tải" }
//    //        }
//    //    },
//    //    new Category
//    //    {
//    //        Id = 2,
//    //        Name = "Xe máy",
//    //        Subcategories = new List<Subcategory>
//    //        {
//    //            new Subcategory { Id = 3, Name = "Xe tay ga" },
//    //            new Subcategory { Id = 4, Name = "Xe số" }
//    //        }
//    //    }
//    //};

//        public async Task<IActionResult> IndexAsync()
//        {
//            var categories = await _categoryRepository.GetAllAsync();
//            return View(categories);    
//        }

//        public IActionResult CreateSubcategory(int categoryId)
//        {
//            var model = new SubcategoryViewModel
//            {
//                CategoryId = categoryId
//            };
//            return View(model);
//        }

//        [HttpPost]
//        public IActionResult CreateSubcategory(SubcategoryViewModel model)
//        {
//            var category = _categories.FirstOrDefault(c => c.Id == model.CategoryId);
//            if (category != null)
//            {
//                var subcategory = new Subcategory
//                {
//                    Id = model.Id,
//                    Name = model.Name
//                };
//                category.Subcategories.Add(subcategory);
//            }
//            return RedirectToAction("Index");
//        }

//        public IActionResult EditSubcategory(int categoryId, int subcategoryId)
//        {
//            var category = _categories.FirstOrDefault(c => c.Id == categoryId);
//            if (category != null)
//            {
//                var subcategory = category.Subcategories.FirstOrDefault(s => s.Id == subcategoryId);
//                if (subcategory != null)
//                {
//                    var model = new SubcategoryViewModel
//                    {
//                        CategoryId = categoryId,
//                        Id = subcategoryId,
//                        Name = subcategory.Name
//                    };
//                    return View(model);
//                }
//            }
//            return RedirectToAction("Index");
//        }

//        [HttpPost]
//        public IActionResult EditSubcategory(SubcategoryViewModel model)
//        {
//            var category = _categories.FirstOrDefault(c => c.Id == model.CategoryId);
//            if (category != null)
//            {
//                var subcategory = category.Subcategories.FirstOrDefault(s => s.Id == model.Id);
//                if (subcategory != null)
//                {
//                    subcategory.Name = model.Name;
//                }
//            }
//            return RedirectToAction("Index");
//        }

//        public IActionResult DeleteSubcategory(int categoryId, int subcategoryId)
//        {
//            var category = _categories.FirstOrDefault(c => c.Id == categoryId);
//            if (category != null)
//            {
//                var subcategory = category.Subcategories.FirstOrDefault(s => s.Id == subcategoryId);
//                if (subcategory != null)
//                {
//                    category.Subcategories.Remove(subcategory);
//                }
//            }
//            return RedirectToAction("Index");
//        }
//    }
//}

*/