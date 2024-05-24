using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            // Hành động này để tạo view
            return View();
        }

        [HttpPost]
        public IActionResult Index(int year)
        {
            // Hành động này để xử lý dữ liệu
            DateTime startDate;
            DateTime endDate;
            DateTime selectedYear;
            if (year < 1900 || year > 2100)
            {
                ViewBag.ErrorMessage = "Năm không hợp lệ.";
                return View();
            }

            startDate = new DateTime(year, 1, 1);
            endDate = startDate.AddYears(1).AddDays(-1); // Kết thúc vào cuối năm đã chọn

            var invoices = _context.Orders
                .Where(h => h.OrderDate >= startDate && h.OrderDate <= endDate)
                .ToList();

            // Tạo mảng để lưu trữ tổng tiền của từng tháng
            decimal[] revenuePerMonth = new decimal[12];
            decimal totalRevenue = 0;

            // Tính toán tổng tiền của các hóa đơn trong từng tháng
            foreach (var invoice in invoices)
            {
                int monthIndex = invoice.OrderDate.Month - 1; // Chỉ số tháng trong mảng (từ 0 đến 11)
                revenuePerMonth[monthIndex] += invoice.TotalPrice; // Cộng tổng tiền của hóa đơn vào tháng tương ứng
                totalRevenue += invoice.TotalPrice;
            }

            var revenueData = revenuePerMonth.Select((value, index) =>
            {
                string label;
                decimal formattedValue;

                if (value < 1000000)
                {
                    formattedValue = value / 1000; // Chuyển đổi sang ngàn
                    label = $"Tháng {index + 1} ngàn";
                }
                else
                {
                    formattedValue = value / 1000000; // Chuyển đổi sang triệu
                    label = $"Tháng {index + 1} triệu";
                }

                return new { Value = formattedValue, Label = label };
            }).ToArray();

            ViewBag.RevenueData = revenueData;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.Year = year;
            // Hiển thị biểu đồ
            return View();
        }
    }
}
