﻿using Microsoft.AspNetCore.Mvc;
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
            ViewBag.productss = products.Take(16);
            ViewBag.productsss = products.Take(20);
            List<Product> cars = products.Where(p => p.Category.Name == "Quà Sinh Nhật").Take(6).ToList();
            ViewBag.Cars = cars;
            List<Product> motors = products.Where(p => p.Category.Name == "test").Take(8).ToList();
            ViewBag.Motors = motors;
            return View(products);
		}
        public IActionResult Contact()
        {
            return View();
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
