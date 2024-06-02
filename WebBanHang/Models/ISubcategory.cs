namespace WebBanHang.Models
{
    public interface ISubcategory
    {
        Task<IEnumerable<Subcategory>> GetAllAsync();
        Task<Subcategory> GetByIdAsync(int id);
        Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryIdAsync(int categoryId);
        Task AddAsync(Subcategory subcategory);
        Task UpdateAsync(Subcategory subcategory);
        Task DeleteAsync(int id);
      
    }
}

