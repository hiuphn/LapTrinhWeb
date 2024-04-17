using DuAnDauTien.Models;
using Microsoft.AspNetCore.Mvc;

namespace DuAnDauTien.Controllers
{
    public class HelloController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult BaiTap1()
        {
            ViewBag.Ten = "Phạm Huỳnh Ngọc Hiếu";
            ViewBag.Mssv = "2180609201";
            ViewBag.Email = "phamhuynhngochieu468@gmail.com";
            ViewBag.SDT = "0906464743";
            ViewData["NgaySinh"] = "10/05/2003";
            return View();
        }
        public IActionResult BaiTap2()
        {
            var sanpham = new SanPhamViewModel()
            {
                TenSP = "1 - Chanel Allure Homme Sport – 100ml EDT",
                GiaSP = 145.79,
                AnhMoTa = "Images/Chanel-Allure-Homme-Sport-100ml.jpg"
            };
            return View(sanpham);
        }
    }
}
