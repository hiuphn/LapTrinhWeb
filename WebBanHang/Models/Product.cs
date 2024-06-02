using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanHang.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Mã sản phẩm")]
        public int Id {  get; set; }

        [DisplayName("Tên sản phẩm")]
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc"), StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 kí tự ")]
        
        public string Name { get; set; }


        [Range(1.000, 1000000000000.000, ErrorMessage = "Giá sản phẩm phải nằm trong khoảng từ 1.000 đến 100000.000")]
        [DisplayName("Giá Bán")]
        public decimal Price {  get; set; }

        [DisplayName("Số lượng")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0.")]
        public int Number { get; set; }

        [DisplayName("Mô tả")]
        public string Description {  get; set; }

        [DisplayName("Hình ảnh")]
        public string? ImageUrl {  get; set; }

        [DisplayName("Nhiều Hình ảnh")]
        public List<ProductImage>? Images { get; set;}

        [ForeignKey("Category")]
        [DisplayName("Mã danh mục")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        [DisplayName("Danh mục con")]
        public int? SubcategoryId { get; set; }
        public Subcategory? Subcategory { get; set; }


        [ForeignKey("Supplier")]
        [DisplayName("Nhà cung cấp")]
        public int SupplierID { get; set; }
        public Supplier? Supplier { get; set; }




    }
}
