using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public string CustomerName { get; set; }
        public bool Sex { get; set; }
        public DateTime BirthDay { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public string Email { get; set; }
        public string? CustomerImage { get; set; }
    }
}
