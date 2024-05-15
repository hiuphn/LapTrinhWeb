using Microsoft.EntityFrameworkCore;

namespace WebBanHang.Models
{
    public class OrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Order> GetOrdersByDate(DateTime filterDate)
        {
            // Lấy danh sách đơn hàng theo ngày tháng
            return _dbContext.Orders
                .Where(o => o.OrderDate.Date == filterDate.Date)
                .ToList();
        }

        public List<Order> GetAllOrders()
        {
            // Lấy tất cả đơn hàng
            return _dbContext.Orders.ToList();
        }
    }
}
