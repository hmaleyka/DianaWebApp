namespace DianaApp.Models
{
    public class Size
    {
        public int Id { get; set; }
        public string sizename { get; set; }
        public List<ProductSize>? sizes { get; set; }
    }
}
