namespace WebBanHang.Models
{
    public interface IProductRespository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Product>> GetBySubCategoryIdAsync(int subcategoryId);
        Task<List<string>> GetImagesByProductIdAsync(int productId);
    }
}
