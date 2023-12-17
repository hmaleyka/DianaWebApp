using DianaApp.DAL;
using Microsoft.AspNetCore.Mvc;

namespace DianaApp.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {

        private AppDbContext _dbcontext;

        public FooterViewComponent(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var setting = _dbcontext.setting.ToDictionary(x=>x.Key, x=>x.Value);
            return View(setting);
        }
    }
}
