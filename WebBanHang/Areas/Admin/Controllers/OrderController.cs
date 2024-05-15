using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;
namespace WebBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class OrderController : Controller
    {

        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.ToListAsync());
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
            var order = _context.Orders.FirstOrDefault(o => o.Id == 1);

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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoaDon = await _context.Orders.FindAsync(id);
            if (hoaDon != null)
            {
                _context.Orders.Remove(hoaDon);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

    }
}
