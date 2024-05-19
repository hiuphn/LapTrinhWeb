using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanHang.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "tên danh mục là bắt buộc")]
        [StringLength(50, ErrorMessage = "Tên danh mục không được vượt quá 50 kí tự ")]

        public string Name { get; set; }


        public List<Product>? Products { get; set; }
        public ICollection<Subcategory> Subcategories { get; set; }

    }
}
