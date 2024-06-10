using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanHang.Models
{
	public class Order
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal TotalPrice { get; set; }
        public string? CustomerName {  get; set; }
		public string ShippingAddress { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^(0|\+84)[3|5|7|8|9]\d{8}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneCheckout { get; set; }
		public string? Notes { get; set; }
        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }
        public string? PhuongThucThanhToan { get; set; }
        public OrderStatus Status { get; set; }
        [ForeignKey("UserId")]
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }
		public List<OrderDetail> OrderDetails { get; set; }
        public Customer? Customer { get; set; }
        public Staff? Staff { get; set; }
    }
    public enum OrderStatus
    {
        Chưa_Giải_Quyết,
        Đã_Xử_Lí,
        Đã_Vận_Chuyển,
        Đã_Giao_Hàng,
        Đã_Hủy
    }
}
