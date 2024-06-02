using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class ExcelController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ExcelController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Export()
        {
            var data = _context.Orders.ToList();
            var stream= new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets.Add("Order");
                // Nạp dữ liệu vào bảng tính
                sheet.Cells[1, 1].Value = "Mã khách hàng";
                sheet.Cells[1, 2].Value = "Tên khách hàng";
                sheet.Cells[1, 3].Value = "Ngày đặt hàng";
                sheet.Cells[1, 4].Value = "Tổng tiền";
                sheet.Cells[1, 5].Value = "Địa chỉ";
                int row = 2;
                foreach (var item in data)
                {
                    sheet.Cells[row, 1].Value=item.UserId;
                    sheet.Cells[row, 2].Value = item.CustomerName;
                    sheet.Cells[row, 3].Value= item.OrderDate;
                    sheet.Cells[row, 4].Value=item.TotalPrice;
                    sheet.Cells[row, 5].Value=item.ShippingAddress;
                    row++;
                }
                // Lưu
                package.Save();
            }
            stream.Position = 0;
            var fileName = $"Order_{DateTime.Now.ToString("yyyyMMdd")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
      
        public IActionResult Index()
        {
            return View();
        }
    }
}
