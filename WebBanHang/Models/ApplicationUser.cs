using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class ApplicationUser : IdentityUser
    {
        
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public DateTime? Age { get; set; }
        public bool Sex { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^(0|\+84)[3|5|7|8|9]\d{8}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }
        public string? AvatarPath { get; set; }
        public int? CustomerID { get; set; }
        public int? StaffId { get; set; }
        public string ChucVu { get; set; }
        public Customer? Customer { get; set; }
        public Staff? Staff { get; set; }
        public Boolean IsLocked { get; set; }
    }
}
