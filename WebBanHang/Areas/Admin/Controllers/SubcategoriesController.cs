using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class SubcategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubcategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var subcategories = _context.Subcategories.Include(s => s.Category);
            return View(await subcategories.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Id,Name,Description,CategoryId")] Subcategory subcategory)
        {
            if (ModelState.IsValid == false)
            {
                
                    _context.Add(subcategory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                
            }
           

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", subcategory.CategoryId);
            return View(subcategory);
        }

    }
}