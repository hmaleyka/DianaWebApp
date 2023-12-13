using DianaApp.Areas.Manage.ViewModels.Product;
using DianaApp.DAL;
using DianaApp.Helpers;
using DianaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DianaApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }
        public async Task <IActionResult> Index()
        {
            List<Product> products = await _dbContext.products.Include(p => p.category)
                .Include(p => p.Sizes)
                .ThenInclude(pt => pt.Size)
                .Include(p => p.Color)
                .ThenInclude(pt => pt.color)
                .Include(p => p.Materials)
                .ThenInclude(pt => pt.material)
                .Include(p => p.Images).ToListAsync();
            return View(products);
        }

        public async Task <IActionResult> Create()
        {
            ViewBag.categories = await _dbContext.categories.ToListAsync();
            ViewBag.colors = await _dbContext.colors.ToListAsync();
            ViewBag.materials = await _dbContext.materials.ToListAsync();
            ViewBag.sizes = await _dbContext.sizes.ToListAsync();

            return View ();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM createProductvm)
        {
            ViewBag.categories = await _dbContext.categories.ToListAsync();
            ViewBag.colors = await _dbContext.colors.ToListAsync();
            ViewBag.materials = await _dbContext.materials.ToListAsync();
            ViewBag.sizes = await _dbContext.sizes.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            bool resultcategory = await _dbContext.categories.AnyAsync(c => c.Id == createProductvm.CategoryId);
            if (!resultcategory)
            {
                ModelState.AddModelError("CategoryId", "there is not such like that category");
                return View();
            }
            Product product = new Product()
            {
                Name = createProductvm.Name,
                Price = createProductvm.Price,
                Description = createProductvm.Description,              
                CategoryId = createProductvm.CategoryId,
                Images = new List<ProductImage>()
            };
            if (createProductvm.SizeIds != null)
            {
                foreach (var sizeId in createProductvm.SizeIds)
                {
                    bool resultSize = await _dbContext.sizes.AnyAsync(c => c.Id == sizeId);
                    if (!resultSize)
                    {
                        ModelState.AddModelError("SizeIds", "there is not such like size here");
                        return View();
                    }


                    ProductSize productsize = new ProductSize()
                    {
                        Product = product,
                        SizeId = sizeId
                    };
                    _dbContext.productsSizes.AddAsync(productsize);

                }
            }
            if (createProductvm.ColorIds != null)
            {
                foreach (var colorId in createProductvm.ColorIds)
                {
                    bool resultColor = await _dbContext.colors.AnyAsync(c => c.Id == colorId);
                    if (!resultColor)
                    {
                        ModelState.AddModelError("SizeIds", "there is not such like size here");
                        return View();
                    }


                    ProductColor productcolor = new ProductColor()
                    {
                        Product = product,
                        ColorId = colorId
                    };
                    _dbContext.productsColors.AddAsync(productcolor);

                }
            }
            if (!createProductvm.Photo.CheckType("image/"))
            {
                ModelState.AddModelError("mainPhoto", "you must only apply the image");
                return View();
            }
            if (!createProductvm.Photo.CheckLong(2097152))
            {
                ModelState.AddModelError("mainPhoto", "picture should be less than 3 mb");
                return View();
            }
            ProductImage photo = new ProductImage()
            {
                
                ImgUrl = createProductvm.Photo.Upload(_env.WebRootPath, @"\Upload\Product\"),
                Product = product
            };
            TempData["Error"] = "";

            product.Images.Add(photo);
            if (createProductvm.additionalImages != null)
            {
                foreach (var photos in createProductvm.additionalImages)
                {
                    if (!photos.CheckType("image/"))
                    {
                        TempData["Error"] += $"{photos.FileName} the format isn't correct \t";
                        continue;

                    }
                    if (!photos.CheckLong(2097152))
                    {
                        TempData["Error"] += $"{photos.FileName} the size of picture is not in right format \t";

                        continue;
                    }

                    ProductImage additionalimg = new ProductImage()
                    {
                        
                        ImgUrl = photos.Upload(_env.WebRootPath, @"\Upload\Product\"),
                        Product = product
                    };
                    product.Images.Add(additionalimg);
                }



            }

            await _dbContext.products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
