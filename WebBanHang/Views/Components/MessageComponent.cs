using Microsoft.AspNetCore.Mvc;

namespace WebBanHang.Views.Components
{
    public class MessageComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string message)
        {
            // Trả về một partial view với dữ liệu message được truyền vào
            return View("Default", message);
        }
    }
}
