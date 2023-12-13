namespace DianaApp.Areas.Manage.ViewModels.Product
{
    public class UpdateProductVM
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public double Price { get; set; }
        public int? CategoryId { get; set; }

        public List<int>? SizeIds { get; set; }
        public List<int>? ColorIds { get; set; }
        public List<int>? MaterialIds { get; set; }

        public IFormFile? photo { get; set; }
        public List<ProductImagesVm>? allproductImages { get; set; }

        public List<int>? ImageIds { get; set; }
    }
    public class ProductImagesVm
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
    }
}
