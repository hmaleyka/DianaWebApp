using DianaApp.Models;

namespace DianaApp.ViewModels
{
    public class HomeVM
    {
        public List<Product> products { get; set; }
        public List<Category> categories { get; set; }
        public List<ProductImage> images { get; set; }
        public List<ProductColor> colors { get; set; }
        public List<ProductSize> sizes { get; set; }
        public List<ProductMaterial> materials { get; set; }
        public List<Slider> sliders { get; set; }
        public List<Subscribe> subscribes { get; set; }
      
    }
}
