using Microsoft.EntityFrameworkCore;

namespace WebBanHang.Models
{
    public class EFProductRepository : IProductRespository
    {
        private readonly ApplicationDbContext _context;
        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetByCategoryIdAsync (int categoryId)
        {
            // Sử dụng ORM hoặc công nghệ truy vấn dữ liệu để lấy danh sách các sản phẩm có categoryId tương ứng từ cơ sở dữ liệu
            var products = await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
            return products;
        }
        public async Task<IEnumerable<Product>> GetBySubCategoryIdAsync(int subcategoryId)
        {
            // Sử dụng ORM hoặc công nghệ truy vấn dữ liệu để lấy danh sách các sản phẩm có categoryId tương ứng từ cơ sở dữ liệu
            var products = await _context.Products.Where(p => p.SubcategoryId == subcategoryId).ToListAsync();
            return products;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Include(x => x.Category).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.Include(x => x.Category).SingleOrDefaultAsync(x=>x.Id == id);
        }
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
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
