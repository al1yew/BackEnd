using Allup.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Allup.Helper;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Allup.Models;
using Allup.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Allup.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Allup.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int? status, int page = 1)
        {
            IQueryable<Product> query = _context.Products.Include(p => p.Category).Include(p => p.Brand);

            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = query.Where(p => p.IsDeleted);
                }
                else if (status == 2)
                {
                    query = query.Where(p => !p.IsDeleted);
                }
            }

            //int itemCount = int.Parse(_context.Settings.FirstOrDefault(s => s.Key == "PageItemsCount").Value);
            int itemCount = 10;

            ViewBag.Status = status;

            return View(PaginationList<Product>.Create(query, page, itemCount));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.BrandsForProducts = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.BrandsForProducts = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain).ToListAsync();

            if (!ModelState.IsValid) return View();

            if (await _context.Products.AnyAsync(p => p.ProductName.ToLower().Trim() == product.ProductName.ToLower().Trim() && !p.IsDeleted))
            {
                ModelState.AddModelError("ProductName", $"{product.ProductName} already exists");
                return View();
            }

            if (!await _context.Brands.AnyAsync(b => !b.IsDeleted && b.Id == product.BrandId))
            {
                ModelState.AddModelError("BrandId", "Select brand");
                return View();
            }

            if (product.CategoryId == null && !await _context.Categories.AnyAsync(c => !c.IsDeleted && !c.IsMain && c.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Select category");
                return View();
            }

            if (product.Files.Count() > 5)
            {
                ModelState.AddModelError("Files", "You can select maximum 5 images");
                return View();
            }

            if (product.MainFile != null)
            {
                if (!product.MainFile.CheckContentType("image/jpeg")
                    && !product.MainFile.CheckContentType("image/jpg")
                    && !product.MainFile.CheckContentType("image/png")
                    && !product.MainFile.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("MainFile", "Main image must be jpg(jpeg) format");
                    return View();
                }

                if (product.MainFile.CheckFileLength(15000))
                {
                    ModelState.AddModelError("MainFile", "Main image size must be 15MB");
                    return View();
                }

                product.MainImage = await product.MainFile.CreateAsync(_env, "assets", "images");
            }
            else
            {
                ModelState.AddModelError("MainFile", "Main image is required");
                return View();
            }

            if (product.HoveredFile != null)
            {
                if (!product.MainFile.CheckContentType("image/jpeg")
                    && !product.MainFile.CheckContentType("image/jpg")
                    && !product.MainFile.CheckContentType("image/png")
                    && !product.MainFile.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("MainFile", "Main image must be jpg(jpeg) format");
                    return View();
                }

                if (product.HoveredFile.CheckFileLength(15000))
                {
                    ModelState.AddModelError("MainFile", "Main image size must be 15MB");
                    return View();
                }

                product.HoverImage = await product.HoveredFile.CreateAsync(_env, "assets", "images");
            }
            else
            {
                ModelState.AddModelError("HoveredFile", "Hovered image is required");
                return View();
            }

            if (product.Files != null && product.Files.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();

                foreach (IFormFile file in product.Files)
                {
                    if (!file.CheckContentType("image/jpeg")
                    && !file.CheckContentType("image/jpg")
                    && !file.CheckContentType("image/png")
                    && !file.CheckContentType("image/gif"))
                    {
                        ModelState.AddModelError("MainFile", "Main image must be jpg(jpeg) format");
                        return View();
                    }

                    if (file.CheckFileLength(15000))
                    {
                        ModelState.AddModelError("MainFile", "Main image size must be 15MB");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = await file.CreateAsync(_env, "assets", "images")
                    };

                    productImages.Add(productImage);
                }

                product.ProductImages = productImages;
            }

            string seria = (_context.Brands.FirstOrDefault(b => b.Id == product.BrandId).BrandName.Substring(0, 2) + product.ProductName.Trim().Substring(0, 2)).ToLower();
            int code = _context.Products.OrderByDescending(p => p.Id).FirstOrDefault(p => p.Seria == seria) != null ? _context.Products.OrderByDescending(p => p.Id).FirstOrDefault(p => p.Seria == seria).Code += 1 : 1;

            product.Code = code;
            product.Seria = seria;
            product.ProductName = product.ProductName.Trim();

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            TempData["success"] = "Product Is Created!";

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            if (product == null) return NotFound();

            ViewBag.BrandsForProducts = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain).ToListAsync();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Product product)
        {
            ViewBag.BrandsForProducts = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain).ToListAsync();

            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            if (id != product.Id) return BadRequest();

            Product dbProduct = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (dbProduct == null) return NotFound();

            if (await _context.Products.AnyAsync(p => p.ProductName.ToLower().Trim() == product.ProductName.ToLower().Trim() && !p.IsDeleted))
            {
                ModelState.AddModelError("ProductName", $"{product.ProductName} already exists");
                return View();
            }

            if (!await _context.Brands.AnyAsync(b => !b.IsDeleted && b.Id == product.BrandId))
            {
                ModelState.AddModelError("BrandId", "Select brand");
                return View();
            }

            if (product.CategoryId == null && !await _context.Categories.AnyAsync(c => !c.IsDeleted && !c.IsMain && c.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Select category");
                return View();
            }

            int canSelectCount = 5 - dbProduct.ProductImages.Count();

            if (product.Files != null && canSelectCount < product.Files.Count())
            {
                ModelState.AddModelError("Files", $"You can select {canSelectCount} items");
                return View();
            }

            if (product.MainFile != null)
            {
                if (!product.MainFile.CheckContentType("image/jpeg")
                    && !product.MainFile.CheckContentType("image/jpg")
                    && !product.MainFile.CheckContentType("image/png")
                    && !product.MainFile.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("MainFile", "Main image must be jpg(jpeg) format");
                    return View();
                }

                if (product.MainFile.CheckFileLength(15000))
                {
                    ModelState.AddModelError("MainFile", "Main image size must be 15MB");
                    return View();
                }

                FileHelper.DeleteFile(_env, dbProduct.MainImage, "assets", "images");

                dbProduct.MainImage = await product.MainFile.CreateAsync(_env, "assets", "images");
            }

            if (product.HoveredFile != null)
            {
                if (!product.MainFile.CheckContentType("image/jpeg")
                    && !product.MainFile.CheckContentType("image/jpg")
                    && !product.MainFile.CheckContentType("image/png")
                    && !product.MainFile.CheckContentType("image/gif"))
                {
                    ModelState.AddModelError("HoveredFile", "Hovered image must be jpg(jpeg) format");
                    return View();
                }

                if (product.HoveredFile.CheckFileLength(15000))
                {
                    ModelState.AddModelError("HoveredFile", "Hovered image size must be 15MB");
                    return View();
                }

                FileHelper.DeleteFile(_env, dbProduct.HoverImage, "assets", "images");

                dbProduct.HoverImage = await product.HoveredFile.CreateAsync(_env, "assets", "images");
            }

            if (product.Files != null && product.Files.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();

                foreach (IFormFile file in product.Files)
                {
                    if (!file.CheckContentType("image/jpeg")
                    && !file.CheckContentType("image/jpg")
                    && !file.CheckContentType("image/png")
                    && !file.CheckContentType("image/gif"))
                    {
                        ModelState.AddModelError("Files", "Images must be image format");
                        return View();
                    }

                    if (product.HoveredFile.CheckFileLength(15000))
                    {
                        ModelState.AddModelError("Files", "Each image's size must be 15MB");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = await file.CreateAsync(_env, "assets", "images")
                    };

                    productImages.Add(productImage);
                }

                dbProduct.ProductImages.AddRange(productImages);
            }

            product.ProductName = product.ProductName.Trim();

            await _context.SaveChangesAsync();

            TempData["success"] = "Product Is Updated!";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id, int? status, int page)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product == null) return NotFound();

            product.IsDeleted = true;
            product.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            IQueryable<Product> query = _context.Products.Include(p => p.Category).Include(p => p.Brand);

            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = query.Where(p => p.IsDeleted);
                }
                else if (status == 2)
                {
                    query = query.Where(p => !p.IsDeleted);
                }
            }

            ViewBag.Status = status;

            //int itemCount = int.Parse(_context.Settings.FirstOrDefault(s => s.Key == "PageItemsCount").Value);

            int itemCount = 10;

            return PartialView("_ProductIndexPartial", PaginationList<Product>.Create(query, page, itemCount));
        }

        public async Task<IActionResult> Restore(int? id, int? status, int page)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            product.IsDeleted = false;
            product.DeletedAt = null;

            await _context.SaveChangesAsync();

            IQueryable<Product> query = _context.Products.Include(p => p.Category).Include(p => p.Brand);

            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = query.Where(p => p.IsDeleted);
                }
                else if (status == 2)
                {
                    query = query.Where(p => !p.IsDeleted);
                }
            }

            ViewBag.Status = status;

            //int itemCount = int.Parse(_context.Settings.FirstOrDefault(s => s.Key == "PageItemsCount").Value);

            int itemCount = 10;

            return PartialView("_ProductIndexPartial", PaginationList<Product>.Create(query, page, itemCount));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteImage(int? id)
        {
            ViewBag.BrandsForProducts = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain).ToListAsync();

            if (id == null) return BadRequest();

            ProductImage productImage = await _context.ProductImages.FirstOrDefaultAsync(p => p.Id == id);

            if (productImage == null) return NotFound();

            Product product = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == productImage.ProductId);

            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();

            FileHelper.DeleteFile(_env, productImage.Image, "assets", "images");

            return PartialView("_ProductImagePartial", product.ProductImages);
        }

        #region my Code

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Product product)
        //{
        //    ViewBag.BrandsForProducts = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
        //    ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain).ToListAsync();

        //    if (!ModelState.IsValid) return View();

        //    if (product.CategoryId == null)
        //    {
        //        ModelState.AddModelError("BrandId", "Select brand correctly");
        //        return View();
        //    }
        //    if (product.BrandId == null)
        //    {
        //        ModelState.AddModelError("CategoryId", "Select brand correctly");
        //        return View();
        //    }

        //    if (await _context.Products.AnyAsync(p => p.ProductName.ToLower().Trim() == product.ProductName.ToLower().Trim() && !p.IsDeleted))
        //    {
        //        ModelState.AddModelError("Name", $"{product.ProductName} already exists");
        //        return View();
        //    }

        //    string brand = _context.Brands.FirstOrDefault(b => b.Id == product.BrandId).BrandName;
        //    string category = _context.Categories.FirstOrDefault(c => c.Id == product.CategoryId).Name;

        //    product.ProductName = product.ProductName.Trim();
        //    product.Seria = (brand.Substring(0, 2) + product.ProductName.Substring(0, 2)).ToLower();
        //    product.Code = No++;
        //    product.IsNewArrival = true;
        //    product.CreatedAt = DateTime.UtcNow.AddHours(4);

        //    if (product.Files != null)
        //    {
        //        List<ProductImage> productImages = new List<ProductImage>();

        //        for (int i = 0; i < product.Files.Count; i++)
        //        {
        //            if (!product.Files[i].CheckContentType("image/jpeg"))
        //            {
        //                ModelState.AddModelError("File", "You can choose only jpeg(jpg) format!");
        //                return View();
        //            }
        //            if (product.Files[i].CheckFileLength(15000))
        //            {
        //                ModelState.AddModelError("File", "File must be 15MB at most!");
        //                return View();
        //            }

        //            ProductImage productImage = new ProductImage
        //            {
        //                Image = await FileManager.CreateAsync(product.Files[i], _env, "assets", "images"),
        //                ProductId = product.Id
        //            };

        //            if (i == 0)
        //            {
        //                product.MainImage = productImage.Image;
        //            }
        //            if (i == 1)
        //            {
        //                product.HoverImage = productImage.Image;
        //            }

        //            productImages.Add(productImage);
        //        }

        //        product.ProductImages = productImages;
        //    }

        //    await _context.Products.AddAsync(product);
        //    await _context.SaveChangesAsync();

        //    TempData["success"] = "Product Is Created";

        //    return RedirectToAction("Index");
        //}

        //[HttpPost]
        //public async Task<IActionResult> Update(int? id, Product product)
        //{
        //    ViewBag.BrandsForProducts = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
        //    ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain).ToListAsync();

        //    if (!ModelState.IsValid) return View();

        //    if (id == null) return BadRequest();

        //    if (id != product.Id) return BadRequest();

        //    Product dbProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        //    if (dbProduct == null) return NotFound();

        //    if (await _context.Products.AnyAsync(p => p.ProductName.ToLower().Trim() == product.ProductName.ToLower().Trim() && !p.IsDeleted))
        //    {
        //        ModelState.AddModelError("Name", $"{product.ProductName} already exists");
        //        return View(product);
        //    }

        //    string brand = _context.Brands.FirstOrDefault(b => b.Id == product.BrandId).BrandName;
        //    string category = _context.Categories.FirstOrDefault(c => c.Id == product.CategoryId).Name;

        //    dbProduct.ProductName = product.ProductName.Trim();
        //    dbProduct.Seria = (brand.Substring(0, 2) + product.ProductName.Substring(0, 2)).ToLower();
        //    dbProduct.Code = No++;
        //    dbProduct.Price = product.Price;
        //    dbProduct.DiscountPrice = product.DiscountPrice;
        //    dbProduct.ExTax = product.ExTax;
        //    dbProduct.CategoryId = product.CategoryId;
        //    dbProduct.BrandId = product.BrandId;
        //    dbProduct.Count = product.Count;
        //    dbProduct.Description = product.Description;
        //    dbProduct.IsBestSeller = product.IsBestSeller;
        //    dbProduct.IsFeature = product.IsFeature;

        //    dbProduct.UpdatedAt = DateTime.UtcNow.AddHours(4);
        //    dbProduct.IsUpdated = true;

        //    //_context.Products.Update(dbProduct);
        //    //await _context.SaveChangesAsync();

        //    if (product.Files != null)
        //    {
        //        List<ProductImage> dbProductImages = await _context.ProductImages.Where(c => c.ProductId == product.Id).ToListAsync();

        //        for (int i = 0; i < dbProductImages.Count; i++)
        //        {
        //            FileHelper.DeleteFile(_env, dbProductImages[i].Image, "assets", "images");
        //            _context.ProductImages.Remove(dbProductImages[i]);
        //            //await _context.SaveChangesAsync();
        //        }

        //        //main image ve hover image silmek lazimdi database den
        //        // icaze ermemek ki 2den az shekil olsun 
        //        // main ve hoveri deyishmek mecburi olmasin
        //        //3 dene input mentigini gurmag
        //        //yoxlanishlar aparmag lazimdi ki update de hamsi mecburidi ya yox,
        //        //data annotationsa baxmag, burda update metodda error message elemek
        //        //evvel 10 shekil yuklesem create de sonra update edende 1 dene yuklesem, 10 shekili silir, databaseden, papkadan silir, amma productun 
        //        //main image ve hover imagesinnen silmir. onlari da silmek lazimdi burda, ve if qoymaq ki yuklenen shekiller minimum 2 dene olsun, seper
        //        // razobratsa s trema inputami i privesti na ekran v view vse fotki update-delayemoqo produkta, s knopkoy X dla udaleniya

        //        List<ProductImage> productImages = new List<ProductImage>();

        //        for (int i = 0; i < product.Files.Count; i++)
        //        {
        //            if (!product.Files[i].CheckContentType("image/jpeg"))
        //            {
        //                ModelState.AddModelError("File", "You can choose only jpeg(jpg) format!");
        //                return View();
        //            }

        //            if (product.Files[i].CheckFileLength(15000))
        //            {
        //                ModelState.AddModelError("File", "File must be 15MB at most!");
        //                return View();
        //            }

        //            ProductImage productImage = new ProductImage
        //            {
        //                Image = await FileManager.CreateAsync(product.Files[i], _env, "assets", "images"),
        //                ProductId = dbProduct.Id
        //            };

        //            if (i == 0)
        //            {
        //                dbProduct.MainImage = productImage.Image;
        //            }
        //            if (i == 1)
        //            {
        //                dbProduct.HoverImage = productImage.Image;
        //            }

        //            productImages.Add(productImage);

        //            //_context.Products.Update(dbProduct);
        //            //_context.ProductImages.Update(productImage);
        //            //await _context.SaveChangesAsync();
        //        }

        //        dbProduct.ProductImages = productImages;
        //    }

        //    await _context.SaveChangesAsync();

        //    TempData["success"] = "Product Is Updated!";

        //    return RedirectToAction("Index");
        //}

        #endregion

    }
}
