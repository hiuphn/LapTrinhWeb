using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanHang.Models
{
    public class Staff
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public bool Sex { get; set; }
        public string Position { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public string StaffPhone { get; set; }
        public string Email { get; set; }
        public string? AvatarPath { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        // Khóa ngoại đến bảng chức vụ (AspNetRoles)
        public string RoleId { get; set; }
        public IdentityRole Role { get; set; }
        // public virtual ApplicationUser User { get; set; }
    }
}
