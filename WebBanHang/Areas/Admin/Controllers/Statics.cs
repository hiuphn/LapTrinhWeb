using Microsoft.AspNetCore.Mvc;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class Statics : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
