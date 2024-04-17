using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebBanHang.Models;

namespace WebBanHang.ViewModels
{
    
    public class CategoryViewModel
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters.")]
        public string Name { get; set; }

        [Display(Name = "Parent Category")]
        public int ParentCategoryId { get; set; }

        public List<SelectListItem> ParentCategories { get; set; }
        private readonly ApplicationDbContext _context;
        
    }
}
