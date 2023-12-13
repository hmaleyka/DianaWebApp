using System.ComponentModel.DataAnnotations;

namespace DianaApp.Areas.Manage.ViewModels.Product
{
    public class CreateProductVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        public double Price { get; set; }
        public int? CategoryId { get; set; }

        public List<int>? SizeIds { get; set; }
        public List<int>? ColorIds { get; set; }
        public List<int>? MaterialIds { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
        public List<IFormFile>? additionalImages { get; set; }
      
     

    }
}
