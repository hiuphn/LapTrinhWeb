using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class HomeController : Controller
    {
		private readonly IProductRespository _productRespository;


		public HomeController(IProductRespository productRespository)
		{
			_productRespository = productRespository;

		}

		public async Task<IActionResult> Index()
		{
            var products = await _productRespository.GetAllAsync();
            ViewBag.productss = products.Take(8);
            ViewBag.productsss = products.Take(6);
            List<Product> cars = products.Where(p => p.Category.Name == "� T�").Take(6).ToList();
            ViewBag.Cars = cars;
            List<Product> motors = products.Where(p => p.Category.Name == "Xe M�y").Take(4).ToList();
            ViewBag.Motors = motors;
            return View(products);
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
