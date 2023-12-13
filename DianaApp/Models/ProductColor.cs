namespace DianaApp.Models
{
    public class ProductColor
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public int? ColorId { get; set; }
        public Color? color { get; set; }

    }
}
