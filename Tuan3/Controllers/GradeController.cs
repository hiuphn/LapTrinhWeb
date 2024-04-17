using Microsoft.AspNetCore.Mvc;
using System.Security.Permissions;
using Tuan3.Models;
namespace Tuan3.Controllers
{
    
    public class GradeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public GradeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Grade> listgrade = _context.Grades.ToList();
            return View(listgrade);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Create(Grade grade)
        {
            _context.Grades.Add(grade);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var item = _context.Grades.SingleOrDefault(x => x.GradeId == id);
            if(item == null)
            {
                return NotFound();
            }

            return View(item);
        }
        [HttpPost]
        
        public IActionResult Edit(Grade grade)
        {
            var item = _context.Grades.SingleOrDefault(x => x.GradeId == grade.GradeId);
            if(item == null)
            {
                return NotFound();
            }
            item.GradeName = grade.GradeName;
            _context.Grades.Update(item);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            var item = _context.Grades.SingleOrDefault(x => x.GradeId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
        public IActionResult Delete(int id)
        {
            var item = _context.Grades.SingleOrDefault(x => x.GradeId == id);
            if(item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]

        public IActionResult Delete(Grade grade)
        {
            var item = _context.Grades.SingleOrDefault(x => x.GradeId == grade.GradeId);
            if( item == null)
            {
                return NotFound();
            }
            _context.Grades.Remove(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        
    }
}
