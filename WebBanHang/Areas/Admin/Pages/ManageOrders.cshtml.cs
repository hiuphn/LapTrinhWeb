using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBanHang.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebBanHang.Areas.Admin.Pages
{
    public class ManageOrders : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public ManageOrders(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Order> Orders { get; set; }

        public void OnGet(string date)
        {
            // Lấy danh sách đơn hàng từ cơ sở dữ liệu
            Orders = new List<Order>(); // Khởi tạo danh sách đơn hàng

            // Kiểm tra xem ngày được truyền vào có hợp lệ hay không
            if (!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out DateTime selectedDate))
            {
                // Lấy danh sách đơn hàng cho ngày cụ thể
                Orders = _dbContext.Orders.Where(o => o.OrderDate.Date == selectedDate.Date).ToList();
            }
            else
            {
                // Lấy danh sách đơn hàng tất cả
                Orders = _dbContext.Orders.ToList();
            }
        }
    }
}
