using DianaApp.DAL;
using DianaApp.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles="Admin, Moderator")]
        public IActionResult Index()
        {
            List<Category> categories = _context.categories.Include(p => p.Products).ToList();
            return View(categories);
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
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
        [Authorize(Roles = "Admin")]
        public IActionResult Update (int Id) 
        {
            Category category = _context.categories.Find(Id);
            return View(category);
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {

            Category category = _context.categories.Find(id);
            _context.categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
