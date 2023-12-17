using DianaApp.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DianaApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="Admin")]
    public class SettingController : Controller
    {

        private AppDbContext _dbcontext;

        public SettingController(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IActionResult> Index()
        {
            List<Setting> setting = await _dbcontext.setting.ToListAsync();
            return View(setting);
        }

        public IActionResult Create ()
        {
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Create (Setting setting)
        {
            Setting settings = new Setting()
            {
                Key = setting.Key,
                Value = setting.Value,
            };
            await _dbcontext.AddAsync(settings);
            await _dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update (int id)
        {
            Setting setting = await _dbcontext.setting.FirstOrDefaultAsync(x => x.Id == id);

            Setting setting1 = new Setting()
            {
                Id = setting.Id,
                Key = setting.Key,
                Value = setting.Value,

            };
            return View(setting);
        }
        [HttpPost]
        public async Task<IActionResult> Update (Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existsetting = await _dbcontext.setting.Where(s => s.Id == setting.Id).FirstOrDefaultAsync();
            existsetting.Key= setting.Key;
            existsetting.Value= setting.Value;
            await _dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete (int id)
        {
            var setting = _dbcontext.setting.FirstOrDefault(x => x.Id == id);
            _dbcontext.setting.Remove(setting);
            _dbcontext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
