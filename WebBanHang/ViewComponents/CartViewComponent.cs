using Microsoft.AspNetCore.Mvc;
/*using WebBanHang.Migrations;*/
using WebBanHang.Models;
using WebBanHang.ViewModels;

namespace WebBanHang.ViewComponents
{
    [ViewComponent(Name = "Cart")]
    public class CartViewComponents : ViewComponent
    {
        private ApplicationDbContext db;

        public CartViewComponents(ApplicationDbContext context) => db = context;
        public IViewComponentResult Invoke()
        {
            var data = db.Products.Select(p => new CartVM
            {
                ProductIdCart = p.Id,
                NameCart = p.Name,
                PriceCart = p.Price,
                ImageUrlCart = p.ImageUrl,

            });
            return View(data);
        }
    }
}
