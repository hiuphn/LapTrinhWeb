using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class StaffViewModel
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public bool Sex { get; set; }
        public string Position { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^(0|\+84)[3|5|7|8|9]\d{8}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string StaffPhone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string? AvatarPath { get; set; }
        public string Password
        {
            get; set;
        }
    }
}
