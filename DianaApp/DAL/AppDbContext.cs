using DianaApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DianaApp.DAL
{
    public class AppDbContext: IdentityDbContext<AppUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 

        }

        public DbSet<Product> products { get; set; }

        public DbSet<ProductImage> productsImage { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Color> colors { get; set; }
        public DbSet<Material> materials { get; set; }
        public DbSet<ProductSize> productsSizes { get; set; }
        public DbSet<ProductColor> productsColors { get; set; }
        public DbSet<ProductMaterial> productsMaterials { get; set; }
        public DbSet<Size> sizes { get; set; }

    }
}
