using Microsoft.AspNetCore.Mvc;
using Tuan2.Models;

namespace Tuan2.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(User user) 
        {
            if(ModelState.IsValid)
            {
                return Content("Đăng kí thành công");
            }
            return View(user);
        }
    }
}
