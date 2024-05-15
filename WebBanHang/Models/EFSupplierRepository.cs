using Microsoft.EntityFrameworkCore;

namespace WebBanHang.Models
{
    public class EFSupplierRepository : ISupplierRespository
    {
        private readonly ApplicationDbContext _context;
        public EFSupplierRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }
        public async Task<Supplier> GetByIdAsync(int id)
        {
            return await _context.Suppliers.SingleOrDefaultAsync(x => x.SupplierID == id);
        }


        public async Task AddAsync(Supplier nhacungcap)
        {
            _context.Suppliers.Add(nhacungcap);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Supplier nhacungcap)
        {
            _context.Suppliers.Update(nhacungcap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Suppliers.FindAsync(id);
            _context.Suppliers.Remove(product);
            await _context.SaveChangesAsync();
        }
        public async Task<List<string>> GetImagesByProductIdAsync(int productId)
        {
            var images = await _context.ProductImages
                .Where(i => i.ProductId == productId)
                .Select(i => i.Url)
                .ToListAsync();

            return images;
        }

        
    }
}
