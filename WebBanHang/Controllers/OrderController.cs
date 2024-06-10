using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;
namespace WebBanHang.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly OrderRepository _orderRepository;
        public OrderController(ApplicationDbContext context, OrderRepository orderRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
        }
        
        public IActionResult Index(int orderId,string user)
        {
            // Lấy danh sách các đơn hàng đã mua, ví dụ từ cơ sở dữ liệu


            var orders = _context.Orders.Where(n => n.UserId ==user).ToList();
            return View(orders);
        }
        
        public IActionResult Track(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        public IActionResult UpdateStatus(int orderId, OrderStatus status)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            _context.SaveChanges();

            return RedirectToAction("Index", new { orderId = order.Id });
        }
        public IActionResult FilterOrders(DateTime? filterDate)
        {
            List<Order> filteredOrders;

            if (filterDate.HasValue)
            {
                // Lọc đơn hàng theo ngày tháng
                filteredOrders = _orderRepository.GetOrdersByDate(filterDate.Value);
            }
            else
            {
                // Lấy tất cả đơn hàng
                filteredOrders = _orderRepository.GetAllOrders();
            }

            return View("Index", filteredOrders);
        }
    }
}
