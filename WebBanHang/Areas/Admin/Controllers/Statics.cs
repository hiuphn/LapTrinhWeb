using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class Statics : Controller
    {

        ApplicationDbContext _context;
        public Statics(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var invoices = _context.Orders.ToList();
            decimal totalRevenue = 0;
            foreach (var invoice in invoices)
            {
                totalRevenue += invoice.TotalPrice;
            }
            var userCount = 0;
            userCount = _context.Customers.Count();
            ViewData["UserCount"] = userCount;
            ViewBag.TotalRevenue = totalRevenue;



            var orderDetails = _context.OrderDetails
         .Include(od => od.Product)
        .ToList();

            // Kiểm tra và nhóm theo ProductId, tính tổng số lượng
            var productSales = orderDetails
                .Where(od => od.Product != null)
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductName = g.FirstOrDefault().Product.Name,
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .ToList();

            ViewBag.ProductSales1 = productSales;
            ViewBag.ProductSaleName1 = productSales.Select(m => m.ProductName).ToList();
            ViewBag.ProductSaleQuantity1 = productSales.Select(m => m.TotalQuantity);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(int year = -1)
        {
            var invoices = _context.Orders.ToList();
            decimal totalRevenue = 0;
            foreach (var invoice in invoices)
            {
                totalRevenue += invoice.TotalPrice;
            }
            var userCount = 0;
            userCount = _context.Customers.Count();
            ViewData["UserCount"] = userCount;
            ViewBag.TotalRevenue = totalRevenue;



            var orderDetails = _context.OrderDetails
            .Include(od => od.Product)
        .ToList();

            // Kiểm tra và nhóm theo ProductId, tính tổng số lượng
          
            if (year != -1)
            {
                DateTime startDate;
                DateTime endDate;
                if (year < 1900 || year > 2100)
                {
                    ViewBag.ErrorMessage = "Năm không hợp lệ.";
                    return View();
                }

                startDate = new DateTime(year, 1, 1);
                endDate = startDate.AddYears(1).AddDays(-1);

                 invoices = _context.Orders
                    .Where(h => h.OrderDate >= startDate && h.OrderDate <= endDate)
                    .ToList();

                decimal[] revenuePerMonth = new decimal[12];
                 totalRevenue = 0;

                foreach (var invoice in invoices)
                {
                    int monthIndex = invoice.OrderDate.Month - 1;
                    revenuePerMonth[monthIndex] += invoice.TotalPrice;
                    totalRevenue += invoice.TotalPrice;
                }

                var revenueData = revenuePerMonth.Select((value, index) =>
                {
                    string label;
                    decimal formattedValue;

                    if (value < 1000000)
                    {
                        formattedValue = value / 1000;
                        label = $"Tháng {index + 1} ngàn";
                    }
                    else
                    {
                        formattedValue = value / 1000000;
                        label = $"Tháng {index + 1} triệu";
                    }

                    return new { Value = formattedValue, Label = label };
                }).ToArray();

                 userCount = await _context.Customers.CountAsync();
                ViewData["UserCount"] = userCount;
                ViewBag.RevenueData = revenueData;
                ViewBag.TotalRevenue = totalRevenue;
                ViewBag.Year = year;

                // Lấy dữ liệu chi tiết các sản phẩm đã bán


                // Kiểm tra và nhóm theo ProductId, tính tổng số lượng
                 orderDetails = _context.OrderDetails
            .Where(od => od.Order.OrderDate >= startDate && od.Order.OrderDate <= endDate)
             .Include(od => od.Product)
            .ToList();
            }
            var productSales = orderDetails
              .Where(od => od.Product != null)
              .GroupBy(od => od.ProductId)
              .Select(g => new
              {
                  ProductName = g.FirstOrDefault().Product.Name,
                  TotalQuantity = g.Sum(od => od.Quantity)
              })
              .ToList();

            ViewBag.ProductSales2 = productSales;
            ViewBag.ProductSaleName2 = productSales.Select(m => m.ProductName).ToList();
            ViewBag.ProductSaleQuantity2 = productSales.Select(m => m.TotalQuantity);

            return View();
        }
        
    }
}
