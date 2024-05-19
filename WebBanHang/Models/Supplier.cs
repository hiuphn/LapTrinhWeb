using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }
        public string CompanyName { get; set; }
        public string? Logo { get; set; }
        public string? Email { get; set; }
        public string? SuplierPhone { get; set; }
        public string? SupplierAddress { get; set; }
        public string? Description { get; set; }
    }
}
