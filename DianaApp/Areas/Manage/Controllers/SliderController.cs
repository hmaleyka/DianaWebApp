using DianaApp.DAL;
using DianaApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DianaApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    public class SliderController : Controller
    {

        private AppDbContext _dbcontext;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext dbcontext, IWebHostEnvironment env)
        {
            _dbcontext = dbcontext;
            _env = env;
        }

        public async Task <IActionResult> Index()
        {
            List<Slider> slider = await _dbcontext.slider.ToListAsync();
            return View(slider);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Create (CreateSliderVM slidervm)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if (!slidervm.Image.CheckType("image/"))
            {
                ModelState.AddModelError("Image", "the type of image is not suitable");
                return View();
            }
            //if (!slidervm.Image.CheckLong(2097152))
            //{
            //    ModelState.AddModelError("Image", "the size of image should not be greater than 2MB");
            //    return View();
            //}
            Slider slider = new Slider()
            {
                Title = slidervm.Title,
                SubTitle = slidervm.SubTitle,
                ImgUrl=slidervm.Image.Upload(_env.WebRootPath, @"\Upload\Slider\")
            };
           await  _dbcontext.slider.AddAsync(slider);
           await _dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update (int id )
        {
            Slider oldslider = await  _dbcontext.slider.FirstOrDefaultAsync(s=>s.Id == id);

            UpdateSliderVM slidervm = new UpdateSliderVM()
            {
                Title = oldslider.Title,
                SubTitle = oldslider.SubTitle,
            };
           
             return View(slidervm);   
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateSliderVM updateslider)
        {

            if (updateslider.ImgUrl != null)
            {
                if (!updateslider.Image.CheckType("image/"))
                {
                    ModelState.AddModelError("Image", "The type of image is nout suitable");
                    return View();
                }
                if (updateslider.Image.CheckLong(2097152))
                {
                    ModelState.AddModelError("Image", "the size of image should not be greater than 2MB");
                    return View();
                }
            }
                
                Slider existslider = await _dbcontext.slider.Where(s => s.Id == updateslider.Id).FirstOrDefaultAsync();

                existslider.Title = updateslider.Title;
                existslider.SubTitle = updateslider.SubTitle;
                existslider.ImgUrl = updateslider.Image.Upload(_env.WebRootPath, @"\Upload\Slider\"); 
                await _dbcontext.SaveChangesAsync();
            

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int id)
        {
            var slider = _dbcontext.slider.FirstOrDefault(s => s.Id == id);
            if (slider != null)
            {
                _dbcontext.slider.Remove(slider);
                _dbcontext.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
