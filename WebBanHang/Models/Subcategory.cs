using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class Subcategory
    {
        

        public int Id { get; set; }
        [Required(ErrorMessage = "Tên danh mục con là bắt buộc")]
        [StringLength(50, ErrorMessage = "Tên danh mục con không được vượt quá 50 kí tự ")]
        public string Name { get; set; }
        [DisplayName("Mã danh mục")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }

}
