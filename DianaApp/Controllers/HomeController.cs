using DianaApp.DAL;
using DianaApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DianaApp.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task <IActionResult> Index()
        {

            HomeVM homevm = new HomeVM()
            {
               
                products = await _context.products.Include(p => p.Images).ToListAsync(),
            };
            return View(homevm);
            
        }
    }
}
