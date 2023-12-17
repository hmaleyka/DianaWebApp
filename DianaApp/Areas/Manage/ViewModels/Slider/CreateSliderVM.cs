namespace DianaApp.Areas.Manage.ViewModels.Slider
{
    public class CreateSliderVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public IFormFile Image { get; set; }

    }
}
