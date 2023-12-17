using DianaApp.DAL;
using Microsoft.AspNetCore.Mvc;

namespace DianaApp.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private AppDbContext _dbContext;

        public SliderViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var slider = await _dbContext.slider.Select(s=>new Slider{ Title = s.Title, SubTitle = s.SubTitle, ImgUrl = s.ImgUrl }).ToListAsync();
            return View(slider);
        }
    }
}
