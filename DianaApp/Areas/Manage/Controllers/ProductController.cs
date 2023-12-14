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
                return View();
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

        public async Task<IActionResult> Update(int id)
        {
            Product product = await _dbContext.products.Include(p => p.category)
                .Include(p => p.Materials)
                .ThenInclude(p => p.material)
                .Include(p => p.Sizes)
                .ThenInclude(p => p.Size)
                .Include(p => p.Color)
                .ThenInclude(p => p.color)
                .Where(p => p.Id == id)
                .Include(p => p.Images).FirstOrDefaultAsync();

            if(product is null)
            {
                return View();
            }
            ViewBag.categories = await _dbContext.categories.ToListAsync();
            ViewBag.colors = await _dbContext.colors.ToListAsync();
            ViewBag.materials = await _dbContext.materials.ToListAsync();
            ViewBag.sizes = await _dbContext.sizes.ToListAsync();

            UpdateProductVM updateProductVM = new UpdateProductVM()
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,             
                CategoryId = product.CategoryId,
                ColorIds = new List<int>(),   
                SizeIds = new List<int>(),   
                MaterialIds = new List<int>(),   
                allproductImages = new List<ProductImagesVm>()
            };
            foreach (var item in product.Sizes)
            {
                updateProductVM.SizeIds.Add((int)item.SizeId);
            }
            foreach (var item in product.Color)
            {
                updateProductVM.ColorIds.Add((int)item.ColorId);
            }
            foreach (var item in product.Materials)
            {
                updateProductVM.MaterialIds.Add((int)item.MaterialId);
            }
            foreach (var item in product.Images)
            {
                ProductImagesVm productImages = new ProductImagesVm()
                {
                    
                    ImgUrl = item.ImgUrl,
                    Id = item.Id,
                };

                updateProductVM.allproductImages.Add(productImages);
            }

            return View(updateProductVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductVM updateproductvm)
        {
            ViewBag.categories = await _dbContext.categories.ToListAsync();
            ViewBag.colors = await _dbContext.colors.ToListAsync();
            ViewBag.materials = await _dbContext.materials.ToListAsync();
            ViewBag.sizes = await _dbContext.sizes.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }

            Product existproduct = await _dbContext.products.Where(p => p.Id == updateproductvm.Id)
                .Include(p => p.Sizes)
                .ThenInclude(p => p.Size)
                .Include(p=>p.Materials)
                .ThenInclude(p=>p.material)
                .Include(p=>p.Color)
                .ThenInclude(p=>p.color)
                .Include(b => b.Images)
                .FirstOrDefaultAsync();
            if (existproduct is null)
            {
                return View("Error");
            }
            bool resultcategory = await _dbContext.categories.AnyAsync(c => c.Id == updateproductvm.CategoryId);
            if (!resultcategory)
            {
                ModelState.AddModelError("CategoryId", "there is not such like that category");
                return View();
            }

            existproduct.Name = updateproductvm.Name;
            existproduct.Description = updateproductvm.Description;
            existproduct.Price = updateproductvm.Price;
            existproduct.CategoryId = updateproductvm.CategoryId;

            if (updateproductvm.SizeIds != null)
            {
                foreach (var sizeId in updateproductvm.SizeIds)
                {
                    bool resultSize = await _dbContext.sizes.AnyAsync(c => c.Id == sizeId);
                    if (!resultSize)
                    {
                        ModelState.AddModelError("TagIds", "there is not such like tag here");
                        return View();
                    }


                }
                //yeni yaranmis tag gelir vm deki id yoxlayir eyni olan

                List<int> createSize;
                if (existproduct.Sizes != null)
                {
                    createSize = updateproductvm.SizeIds.Where(ti => !existproduct.Sizes.Exists(pt => pt.SizeId == ti)).ToList();

                }
                else
                {
                    createSize = updateproductvm.SizeIds.ToList();
                }

                foreach (var sizeid in createSize)
                {
                    ProductSize productTag = new ProductSize()
                    {
                        SizeId = sizeid,
                        ProductId = existproduct.Id
                    };
                    //existproduct.productTags.Add(productTag);

                    await _dbContext.productsSizes.AddAsync(productTag);

                }

                List<ProductSize> removeSize = existproduct.Sizes.Where(pt => !updateproductvm.SizeIds.Contains((int)pt.SizeId)).ToList();

                _dbContext.productsSizes.RemoveRange(removeSize);

            }
            else
            {
                var productSizeList = _dbContext.productsSizes.Where(pt => pt.ProductId == existproduct.Id).ToList();
                _dbContext.productsSizes.RemoveRange(productSizeList);

            }

            if (updateproductvm.ColorIds != null)
            {
                foreach (var colorId in updateproductvm.ColorIds)
                {
                    bool resultColor = await _dbContext.sizes.AnyAsync(c => c.Id == colorId);
                    if (!resultColor)
                    {
                        ModelState.AddModelError("ColorIds", "there is not such like color here");
                        return View();
                    }


                }
                //yeni yaranmis tag gelir vm deki id yoxlayir eyni olan

                List<int> createColor;
                if (existproduct.Color != null)
                {
                    createColor = updateproductvm.ColorIds.Where(ti => !existproduct.Color.Exists(pt => pt.ColorId == ti)).ToList();

                }
                else
                {
                    createColor = updateproductvm.ColorIds.ToList();
                }

                foreach (var colorid in createColor)
                {
                    ProductColor productcolor = new ProductColor()
                    {
                        ColorId = colorid,
                        ProductId = existproduct.Id
                    };
                    //existproduct.productTags.Add(productTag);

                    await _dbContext.productsColors.AddAsync(productcolor);

                }

                List<ProductColor> removeColor = existproduct.Color.Where(pt => !updateproductvm.ColorIds.Contains((int)pt.ColorId)).ToList();

                _dbContext.productsColors.RemoveRange(removeColor);

            }
            else
            {
                var productColorList = _dbContext.productsColors.Where(pt => pt.ProductId == existproduct.Id).ToList();
                _dbContext.productsColors.RemoveRange(productColorList);

            }

            if (updateproductvm.MaterialIds != null)
            {
                foreach (var materialId in updateproductvm.MaterialIds)
                {
                    bool resultMaterial = await _dbContext.sizes.AnyAsync(c => c.Id == materialId);
                    if (!resultMaterial)
                    {
                        ModelState.AddModelError("MaterialIds", "there is not such like material here");
                        return View();
                    }


                }
                //yeni yaranmis tag gelir vm deki id yoxlayir eyni olan

                List<int> createMaterial;
                if (existproduct.Materials != null)
                {
                    createMaterial = updateproductvm.MaterialIds.Where(ti => !existproduct.Materials.Exists(pt => pt.MaterialId == ti)).ToList();

                }
                else
                {
                    createMaterial = updateproductvm.MaterialIds.ToList();
                }

                foreach (var materialid in createMaterial)
                {
                    ProductMaterial productmaterial = new ProductMaterial()
                    {
                        MaterialId = materialid,
                        ProductId = existproduct.Id
                    };
                    //existproduct.productTags.Add(productTag);

                    await _dbContext.productsMaterials.AddAsync(productmaterial);

                }

                List<ProductMaterial> removeMaterial = existproduct.Materials.Where(pt => !updateproductvm.MaterialIds.Contains((int)pt.MaterialId)).ToList();

                _dbContext.productsMaterials.RemoveRange(removeMaterial);

            }
            else
            {
                var productMaterialList = _dbContext.productsMaterials.Where(pt => pt.ProductId == existproduct.Id).ToList();
                _dbContext.productsMaterials.RemoveRange(productMaterialList);

            }

            TempData["Error"] = "";

            if (updateproductvm.photo != null)
            {
                if (!updateproductvm.photo.CheckType("image/"))
                {
                    ModelState.AddModelError("mainPhoto", "you must only apply the image");
                    return View();
                }
                if (!updateproductvm.photo.CheckLong(2097152))
                {
                    ModelState.AddModelError("mainPhoto", "picture should be less than 3 mb");
                    return View();
                }
               
                var oldPhoto = existproduct.Images?.FirstOrDefault();
                existproduct.Images?.Remove(oldPhoto);
                ProductImage newproductimage = new ProductImage()
                {
                    ProductId = existproduct.Id,
                    ImgUrl = updateproductvm.photo.Upload(_env.WebRootPath, @"\Upload\Product\")


                };
                existproduct.Images?.Add(newproductimage);
            }
           
            //if (updateproductvm.ImageIds == null)
            //{
            //    existproduct.Images.RemoveAll();
            //}
            //else
            //{
            //    var removeListImage = existproduct.Images?.Where(p => !updateproductvm.ImageIds.Contains(p.Id)).ToList();
            //    if (removeListImage != null)
            //    {
            //        foreach (var image in removeListImage)
            //        {
            //            existproduct.Images.Remove(image);
            //            FileManager.DeleteFile(image.ImgUrl, _env.WebRootPath, @"\Upload\Product\");
            //        }


            //    }
            //    else
            //    {
            //        existproduct.Images.RemoveAll(p => p.ImgUrl);
            //    }

            //}
            //if (updateproductvm.additionalphotos != null)
            //{
            //    foreach (var photo in updateproductvm.additionalphotos)
            //    {
            //        if (!photo.CheckType("image/"))
            //        {
            //            TempData["Error"] += $"{photo.FileName} the format isn't correct \t";
            //            continue;

            //        }
            //        if (!photo.CheckLong(2097152))
            //        {
            //            TempData["Error"] += $"{photo.FileName} the size of picture is not in right format \t";

            //            continue;
            //        }

            //        ProductImage additionalphotos = new ProductImage()
            //        {

            //            ImgUrl = photo.Upload(_env.WebRootPath, @"\Upload\Product\"),
            //            Product = existproduct
            //        };
            //        existproduct.Images?.Add(additionalphotos);

            //    }
            //}




            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));




        }

        public IActionResult Delete(int id)
        {

            var product = _dbContext.products.FirstOrDefault(p => p.Id == id);
            {
                return View("Error");
            }
            
            _dbContext.SaveChanges();
            return Ok();

        }
    }
}
