using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class Discount
    {
        [Key]
        public int DiscountId { get; set; }

        [Required]
        [StringLength(100)]
        public string Code { get; set; }

        [Required]
        public DiscountType Type { get; set; }

        [Required]
        public decimal Value { get; set; } // Giá trị của khuyến mãi: số tiền hoặc phần trăm


        public enum DiscountType
        {
            Percentage, // Giảm giá theo phần trăm
            Amount // Giảm giá theo số tiền
        }
    }
}
