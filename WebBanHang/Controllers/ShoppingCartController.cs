using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using System.Globalization;
using WebBanHang.Data;
using WebBanHang.Extensions;
using WebBanHang.Models;
using WebBanHang.ServicesPay;
using WebBanHang.ViewModels;

namespace WebBanHang.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IProductRespository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVnPayService _vnPayService;
        private readonly EmailService _emailService;
        private readonly ILogger<ShoppingCartController> _logger;
        public ShoppingCartController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager, IProductRespository
        productRepository, EmailService emailService, IVnPayService vnPayService, ILogger<ShoppingCartController> logger)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _vnPayService = vnPayService;
            _logger = logger;
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
/*            			       return Json(Response);
*/           return RedirectToAction("Index");
       }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var category = await _productRepository.GetByIdAsync(productId);
            if (category == null)
            {
                return NotFound();
            }

            return Ok();
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new
    ShoppingCart();
            ViewBag.Cart = cart;
            var discountCodes = _context.Discounts.ToList();
            ViewBag.DiscountCodes = new SelectList(discountCodes, "DiscountId", "Code");
            return View(cart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyDiscount(int discountCode)
        {
            _logger.LogInformation("ApplyDiscount method hit. DiscountCode: {DiscountCode}", discountCode); // Add logging

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.DiscountId == discountCode);

            if (discount != null)
            {
                if (discount.Type.ToString() == "Percentage")
                {
                    cart.DiscountAmount = cart.Items.Sum(item => item.Price * item.Quantity) * (discount.Value / 100);
                }
                else if (discount.Type.ToString() == "Amount")
                {
                    cart.DiscountAmount = discount.Value;
                }
            }
            else
            {
                ModelState.AddModelError("", "Mã giảm giá không hợp lệ.");
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return RedirectToAction("Index");
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

        /*        public IActionResult EmptyCart()
                {
                    return View();
                }*/
        public IActionResult RemoveAll()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }

        public IActionResult EmptyCart()
        {
            return View();

        }
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult OrderCompleted(int orderId)
        {
            var order = _context.Orders.Include(o => o.ApplicationUser).FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public IActionResult Checkout()
        {
            var discountCodes = _context.Discounts.ToList();
            ViewBag.DiscountCodes = new SelectList(discountCodes, "DiscountId", "Code");
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                return View("EmptyCart");
            }

            ViewBag.Cart = cart.Items;
            return View(new Order());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Order hoadon, string payment, int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index");
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                hoadon.UserId = user.Id;
                hoadon.OrderDate = DateTime.UtcNow;
                hoadon.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
                hoadon.OrderDetails = cart.Items.Select(i => new OrderDetail
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList();
                _context.Orders.Add(hoadon);
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("Cart");

                var patientEmail = user.Email;
                var emailSubject = "Order Confirmation";
                var currentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                var TongTien = hoadon.TotalPrice;
                var emailMessage = string.Empty;

                if (payment == "VN-PAY")
                {
                    var vnPayModel = new VnPaymentRequestModel()
                    {
                        Amount = (double)hoadon.TotalPrice,
                        CreateDate = DateTime.Now,
                        Description = "Order Payment",
                        OrderId = hoadon.Id
                    };

                    emailMessage = $"<h1>Invoice</h1><br>" +
                                   $"Your order has been successfully paid at {currentTime}.<br>" +
                                   $"Total amount paid is {TongTien.ToString("C", new CultureInfo("vi-VN"))}.<br>" +
                                   $"Your order will be delivered soon.<br>" +
                                   $"Order details:<br>";

                    hoadon.PhuongThucThanhToan = "VN-PAY";
                    await _context.SaveChangesAsync();
                    await _emailService.SendEmailAsync(patientEmail, emailSubject, emailMessage);
                    return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
                }
                else
                {
                    emailMessage = $"<h1>Invoice</h1><br>" +
                                   $"You have an outstanding invoice.<br>" +
                                   $"Your order was placed successfully at {currentTime}.<br>" +
                                   $"Total amount due is {TongTien.ToString("C", new CultureInfo("vi-VN"))}.<br>" +
                                   $"Please pay the amount upon delivery.<br>" +
                                   $"Order details:<br>";

                    hoadon.PhuongThucThanhToan = "COD";
                    await _context.SaveChangesAsync();
                    await _emailService.SendEmailAsync(patientEmail, emailSubject, emailMessage);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }

            return RedirectToAction("OrderCompleted", new { orderId = hoadon.Id });
        }





        /*        public IActionResult Checkout()
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
                *//*public async Task<IActionResult> Checkout(Order order)
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
                }*//*

                [HttpPost]
                public async Task<IActionResult> Checkout(Order hoadon, string payment)
                {

                    var cart =
                    HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
                    if (cart == null || !cart.Items.Any())
                    {
                        // Xử lý giỏ hàng trống...
                        return RedirectToAction("Index");
                    }
                    try
                    {

                        var user = await _userManager.GetUserAsync(User);
                        hoadon.UserId = user.Id;
                        hoadon.OrderDate = DateTime.UtcNow;
                        decimal TongTien = cart.Items.Sum(i => i.Price * i.Quantity);
                        hoadon.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
                        var currentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        hoadon.OrderDetails = cart.Items.Select(i => new OrderDetail
                        {
                            ProductId = i.ProductId,
                            Quantity = i.Quantity,
                            Price = i.Price
                        }).ToList();
                        _context.Orders.Add(hoadon);
                        await _context.SaveChangesAsync();
                        HttpContext.Session.Remove("Cart");

                        if (payment == "VN-PAY")
                        {
                            var vnPayModel = new VnPaymentRequestModel()
                            {
                                Amount = (double)hoadon.TotalPrice,
                                CreateDate = DateTime.Now,
                                Description = "Thanh toán viện phí",
                                OrderId = hoadon.Id

                            };
                            var patientEmail = hoadon.ApplicationUser.Email; // Replace with the patient's email
                            var emailSubject = "Thanh toán hóa đơn";
                            var emailMessage =  $"<h1>Hóa đơn</h1<br><br>" +
                                                $"Bạn đã thanh toán thành công Hóa đơn .<br><br> " +
                                                $"Bạn đã thanh toán thành công hóa đơn vào lúc {currentTime}.<br><br>" +
                                                $"Tổng tiền đã thanh toán là {TongTien.ToString("C", new CultureInfo("vi-VN"))}<br><br>" +
                                                $"Đơn hàng sẽ sớm về với bạn.<br><br>"+
                                                $"Thông tin chi tiết:<br><br>";

                            hoadon.PhuongThucThanhToan = "VN-PAY";

                            await _context.SaveChangesAsync();
                            await _emailService.SendEmailAsync(patientEmail, emailSubject, emailMessage);
                            return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
                        }
                        else
                        {
                            var patientEmail = hoadon.ApplicationUser.Email; // Replace with the patient's email
                            var emailSubject = "Thanh toán hóa đơn";
                            var emailMessage =  $"<h1>Hóa đơn</h1><br>" +
                                                $"BẠN CÓ HÓA ĐƠN CẦN THANH TOÁN .<br><br> " +
                                                $"Bạn đã đặt đơn hàng thành công vào lúc {currentTime}.<br><br>" +
                                                $"Tổng tiền chưa thanh toán là {TongTien.ToString("C", new CultureInfo("vi-VN"))}<br><br>" +
                                                $"Bạn cần thanh toán số tiền khi nhận được hàng.<br><br>" +
                                                $"Thông tin chi tiết:<br><br>";

                            hoadon.PhuongThucThanhToan = "COD";

                            await _context.SaveChangesAsync();
                            await _emailService.SendEmailAsync(patientEmail, emailSubject, emailMessage);
                        }
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!HoaDonExists(hoadon.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return View("OrderCompleted", hoadon.Id); // Trang xác nhận hoàn thành đơn hàng



                }*/

        /*    public ActionResult Checkout1()
                {
                    CartItem cartItem = new CartItem();
                    Order order = new Order();

                            var data = Tuple.Create(cartItem, order);
                            return View(data);
                        }*/

        /*                public IActionResult UpdateQuantity(int productId, int quantity)
                        {
                            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
                            var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);
                            if (cartItem != null)
                            {
                                cartItem.Quantity = quantity;
                                HttpContext.Session.SetObjectAsJson("Cart", cart);
                            }
                            return RedirectToAction("Index");
                        }*/
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
        private bool HoaDonExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
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
        /*            public IActionResult RemoveAll()
                {
                    HttpContext.Session.Remove("Cart");
                    return RedirectToAction("Index");
                }*/

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
        /*                public ActionResult Checkout1()
                {
                    CartItem cartItem = new CartItem();
                    Order order = new Order();
		}*/

        /*	public IActionResult Checkout()
            {
                var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
                if (cart == null || cart.Items.Count == 0)
                {
                    return View("EmptyCart");
                }
                List<CartItem> cartItem = cart.Items;
                ViewBag.Cart = cartItem;
                return View(new Order());
            }*/

    }
}



