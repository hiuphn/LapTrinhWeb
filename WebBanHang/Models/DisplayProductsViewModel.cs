namespace WebBanHang.Models
{
    public class DisplayProductsViewModel
    {
        public Category Category { get; set; }
        public IEnumerable<Product> Products { get; set; }
       
    }
}
