namespace DianaApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; } 
        public int? CategoryId { get; set; }
        public Category? category { get; set; }

        public List<ProductColor>? Color { get; set; }
        public List<ProductImage>? Images { get; set; }
        public List<ProductMaterial>? Materials { get; set; }
        public  List<ProductSize>? Sizes { get; set; }

    }
}
