using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tuan2.Models
{
    public class User
    {
        [DisplayName("Tên đăng nhập")]
        [Required(ErrorMessage = "Bắt buộc nhập tên đăng nhập")]
        [StringLength(50, MinimumLength = 3, ErrorMessage ="Tên đăng nhập phải từ 3 - 50 kí tự")]
        public string UserName { get; set; }

        [DisplayName("Địa chỉ Email")]
        [Required(ErrorMessage = "Bắt buộc nhập Email")]
        [EmailAddress(ErrorMessage ="Email không hợp lệ")]
        public string Email { get; set; }

        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "Bắt buộc nhập tên đăng nhập")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải ít nhất 6 kí tự")]
        public string Password { get; set; }

        [DisplayName("Xác nhận mật khẩu")]
        [Required(ErrorMessage = "Bắt buộc nhập tên đăng nhập")]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không chính xác")]
        public string ConfirmPassword { get; set; }
    }
}
