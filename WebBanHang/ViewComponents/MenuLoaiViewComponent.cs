using Microsoft.AspNetCore.Mvc;
using WebBanHang.Models;
using WebBanHang.ViewModels;

namespace WebBanHang.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private ApplicationDbContext db;

        public MenuLoaiViewComponent(ApplicationDbContext context) => db = context;
        public IViewComponentResult Invoke()
        {
            var data = db.Categories.Select(p => new MenuLoaiVM     
            {
                IdCategory= p.Id, 
                NameCategory = p.Name, 
                QuantityCategory = p.Products.Count 
            });
            return View(data);
        }
    }
}
