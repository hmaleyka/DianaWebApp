namespace DianaApp.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string materialName { get; set; }
        public List<ProductMaterial>? materials { get; set; }
    }
}
