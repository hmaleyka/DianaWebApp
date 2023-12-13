using DianaApp.DAL;
using DianaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DianaApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategoryController : Controller
    {
        AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
    
        public IActionResult Index()
        {
            List<Category> categories = _context.categories.Include(p => p.Products).ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _context.categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
           
        }

        public IActionResult Update (int Id) 
        {
            Category category = _context.categories.Find(Id);
            return View(category);
        }
        [HttpPost]
        public IActionResult Update(Category newcategory)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            Category oldcategory = _context.categories.Find(newcategory.Id);
            oldcategory.Name = newcategory.Name;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {

            Category category = _context.categories.Find(id);
            _context.categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
