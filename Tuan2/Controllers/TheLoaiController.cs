using Microsoft.AspNetCore.Mvc;

namespace Tuan2.Controllers
{
    public class TheLoaiController : Controller
    {
        public IActionResult Show(List<string> categories)
        {
            string contents = "category List: ";
            foreach (var category in categories)
            {
                contents += string.Format("{0},", category);
            }
            return Content(contents);
        }
    }
}
