namespace WebBanHang.Models
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal DiscountAmount { get; set; }
        public decimal Total => Items.Sum(item => item.Price * item.Quantity) - DiscountAmount;
        public decimal GetTotal()
        {
            decimal total = 0;
            foreach (var item in Items)
            {
                total += item.Quantity * item.Price;
            }
            return total;
        }
        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId ==
            item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }
        public void RemoveItem(int productId)
        {
            Items.RemoveAll(i => i.ProductId == productId);
        }
        // Các phương thức khác...
    }
}
