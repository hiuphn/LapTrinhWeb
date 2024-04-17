namespace WebBanHang.Models
{
    public class DisplayProductdetail
    {
        public Subcategory subcategory { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
