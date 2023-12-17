using System.ComponentModel.DataAnnotations;

namespace DianaApp.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Required, StringLength(25, ErrorMessage = "Length should have max 25 characters")]
        public string SubTitle { get; set; }
        public string? ImgUrl { get; set; }

    }
}
