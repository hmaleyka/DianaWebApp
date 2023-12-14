using DianaApp.DAL;
using DianaApp.Models;
using DianaApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DianaApp.Controllers
{
    public class ShopController : Controller
    {
        AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail(int? id)
        {
            if (id == null) return BadRequest();
            Product product = _context.products
               .Include(p => p.category)
               .Include(p => p.Images)
               .Include(p => p.Sizes)
               .ThenInclude(pt => pt.Size)
               .Include(p => p.Color)
               .ThenInclude(pt => pt.color)
               .Include(p => p.Materials)
               .ThenInclude(pt => pt.material)
               .FirstOrDefault(product => product.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            DetailVM detailvm = new DetailVM()
            {
                product = product,
                products = _context.products.Include(p => p.Images).Include(p => p.category).Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id).ToList()
            };

            return View(detailvm);

        }
    }
}
