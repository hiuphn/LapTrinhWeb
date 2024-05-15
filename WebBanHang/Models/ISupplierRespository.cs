namespace WebBanHang.Models
{
    public interface ISupplierRespository
    {
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier> GetByIdAsync(int id);
        Task AddAsync(Supplier nhacungcap);
        Task UpdateAsync(Supplier nhacungcap);
        Task DeleteAsync(int id);

    }
}
