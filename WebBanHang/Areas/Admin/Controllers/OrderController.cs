using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebBanHang.Models;
using X.PagedList;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class OrderController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly OrderRepository _orderRepository;
        public OrderController(ApplicationDbContext context, OrderRepository orderRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
        }
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public async Task<IActionResult> Index(string searchString, DateTime? filterDate, int? page, int? pageSize)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentFilterDate"] = filterDate;
            int defaultPageSize = pageSize ?? 10; // Default page size is 10 if not provided
            int pageNumber = page ?? 1; // Default page number is 1 if not provided

            var ordersQuery = _context.Orders.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                ordersQuery = ordersQuery.Where(n => n.CustomerName.ToString().Contains(searchString.ToLower()));
            }

            if (filterDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate.Date == filterDate.Value.Date);
            }

            var pagedOrders = await ordersQuery.ToPagedListAsync(pageNumber, defaultPageSize);

            ViewBag.PageSize = new SelectList(new List<int> { 10, 20, 50 }, defaultPageSize);
            ViewBag.CurrentPageSize = defaultPageSize;

            return View(pagedOrders);
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


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // POST: Admin/HoaDons/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discounts = await _context.Discounts.FindAsync(id);
            if (discounts == null)
            {
                return NotFound();
            }

            _context.Discounts.Remove(discounts);
            await _context.SaveChangesAsync();

            return Ok();
        }
        public IActionResult FilterOrders(DateTime? filterDate)
        {
            /*List<Order> filteredOrders;

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

            return View("Index", filteredOrders);*/

            var filteredOrders = _context.Orders.AsQueryable();

            if (filterDate.HasValue)
            {
                filteredOrders = filteredOrders
                    .Where(o => o.OrderDate.Date == filterDate.Value.Date);
            }

            var result = filteredOrders.ToList();

            return View("Index", result);
        }

        private bool HoaDonExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

    }
}
