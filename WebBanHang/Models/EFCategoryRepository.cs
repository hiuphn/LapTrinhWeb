using Microsoft.EntityFrameworkCore;

namespace WebBanHang.Models
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public EFCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        public async Task AddAsync(Category categoy)
        {
            _context.Categories.Add(categoy);
            await _context.SaveChangesAsync();
        }
        

        public async Task UpdateAsync(Category categoy)
        {
            _context.Categories.Update(categoy);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }

}

