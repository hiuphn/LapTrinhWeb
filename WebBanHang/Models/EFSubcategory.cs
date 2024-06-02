using Microsoft.EntityFrameworkCore;

namespace WebBanHang.Models
{
    public class EFSubcategory : ISubcategory
    {
        private readonly ApplicationDbContext _context;
        public EFSubcategory(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Subcategory>> GetAllAsync()
        {
            return await _context.Subcategories.ToListAsync();
        }

        public async Task<Subcategory> GetByIdAsync(int id)
        {
            return await _context.Subcategories.SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryIdAsync(int categoryId)
        {
            return await _context.Subcategories
                                 .Where(s => s.CategoryId == categoryId)
                                 .ToListAsync();
        }
        public async Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryIdAsync(int categoryId)
        {
            return await _context.Subcategorys
                                 .Where(s => s.CategoryId == categoryId)
                                 .ToListAsync();
        }
        public async Task AddAsync(Subcategory subcategory)
        {
            _context.Subcategories.Add(subcategory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Subcategory subcategory)
        {
            _context.Subcategories.Update(subcategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var subcategory = await _context.Subcategories.FindAsync(id);
            _context.Subcategories.Remove(subcategory);
            await _context.SaveChangesAsync();
        }
    }

}