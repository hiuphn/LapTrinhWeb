using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using WebBanHang.Extensions;
using WebBanHang.Models;
using WebBanHang.ViewModels;

namespace WebBanHang.Controllers
{
	[Authorize]
    public class ShoppingCartController : Controller
    {
		private readonly IProductRespository _productRepository;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		public ShoppingCartController(ApplicationDbContext context,
		UserManager<ApplicationUser> userManager, IProductRespository
		productRepository)
		{
			_productRepository = productRepository;
			_context = context;
			_userManager = userManager;
		}
		public async Task<IActionResult> AddToCart(int productId, int quantity)
		{
			// Giả sử bạn có phương thức lấy thông tin sản phẩm từ productId
			var product = await GetProductFromDatabase(productId);
			var order = await GetProductFromDatabase(productId);
            var cartItem = new CartItem
			{
				ProductId = productId,
				Name = product.Name,
				Price = product.Price,
				Quantity = quantity,
				ImageUrl = product.ImageUrl,
				
			};
			var cart =
			HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
			cart.AddItem(cartItem);
			HttpContext.Session.SetObjectAsJson("Cart", cart);

		                      return RedirectToAction("Index");

            /*			       return Json(new { success = true, message = "Product added to cart." });
            */
                }
                public IActionResult Index()
                {
                    var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new
                    ShoppingCart();
                    ViewBag.Cart = cart;
                    return View(cart);
                }
                // Các actions khác...
                private async Task<Product> GetProductFromDatabase(int productId)
                {
                    // Truy vấn cơ sở dữ liệu để lấy thông tin sản phẩm
                    var product = await _productRepository.GetByIdAsync(productId);
                    return product;
                }
                public IActionResult RemoveFromCart(int productId)
                {
                    var cart =
                    HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
                    if (cart is not null)
                    {
                        cart.RemoveItem(productId);
                        // Lưu lại giỏ hàng vào Session sau khi đã xóa mục
                        HttpContext.Session.SetObjectAsJson("Cart", cart);
                    }
                    return RedirectToAction("Index");
                }

                public IActionResult EmptyCart()
                {
                    return View();

                }
                public IActionResult Checkout()
                {
                    var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
                    if (cart == null || cart.Items.Count == 0)
                    {
                        return View("EmptyCart"); 
                    }
                    List<CartItem> cartItem = cart.Items;
                    ViewBag.Cart = cartItem;
                    return View(new Order());
                }
                [HttpPost]
                public async Task<IActionResult> Checkout(Order order)
                {

                    var cart =
                    HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
                    if (cart == null || !cart.Items.Any())
                    {
                        // Xử lý giỏ hàng trống...
                        return RedirectToAction("Index");
                    }
                    var user = await _userManager.GetUserAsync(User);
                    order.UserId = user.Id;
                    order.OrderDate = DateTime.UtcNow;
                    order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
                    order.OrderDetails = cart.Items.Select(i => new OrderDetail
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList();
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.Remove("Cart");

                    return View("OrderCompleted", order.Id); // Trang xác nhận hoàn thành đơn hàng
                }
                public ActionResult Checkout1()
                {
                    CartItem cartItem = new CartItem();
                    Order order = new Order();

                    var data = Tuple.Create(cartItem, order);
                    return View(data);
                }

                public IActionResult UpdateQuantity(int productId, int quantity)
                {
                    var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
                    var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);
                    if (cartItem != null)
                    {
                        cartItem.Quantity = quantity;
                        HttpContext.Session.SetObjectAsJson("Cart", cart);
                    }
                    return RedirectToAction("Index");
                }

               /* public IActionResult TotalPrice()
                {
                    var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
                    decimal totalPrice = cart.Items.Sum(item => item.Price * item.Quantity);
                    return View(totalPrice);
                }
                [HttpGet("totalPrice")]
                public IActionResult TotalPrice(CartItem cartitem)
                {

                    decimal totalPrice = 0;
                    foreach (var item in cartitem)
                    {
                        totalPrice += item.Price * item.Quantity;
                    }
                    return Ok(totalPrice);
                }*/
            public IActionResult RemoveAll()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }
        
    }
}
    

