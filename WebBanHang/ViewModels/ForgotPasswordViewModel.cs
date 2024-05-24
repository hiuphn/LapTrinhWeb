using System.ComponentModel.DataAnnotations;

namespace WebBanHang.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
