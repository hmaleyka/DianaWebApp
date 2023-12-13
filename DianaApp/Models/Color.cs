namespace DianaApp.Models
{
    public class Color
    {
        public int Id { get; set; }
        public string color { get; set; }
        public List<ProductColor>? colors { get; set; }
    }
}
