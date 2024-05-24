using X.PagedList;

namespace WebBanHang.Models
{
    public class DisplayProductsViewModel
    {
        public Category Category { get; set; }
        public IPagedList<Product> Products { get; set; }
    }
}