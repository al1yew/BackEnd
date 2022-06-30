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
//using Allup.DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace Allup.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        public int No = 1;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int? status, int page = 1)
        {
            IQueryable<Product> query = _context.Products;

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
                ModelState.AddModelError("Name", $"{product.ProductName} already exists");
                return View();
            }

            string brand = _context.Brands.FirstOrDefault(b => b.Id == product.BrandId).BrandName;
            string category = _context.Categories.FirstOrDefault(c => c.Id == product.CategoryId).Name;

            product.ProductName = product.ProductName.Trim();
            product.Seria = (brand.Substring(0, 2) + product.ProductName.Substring(0, 2)).ToLower();
            product.Code = No++;
            product.IsNewArrival = true;
            product.CreatedAt = DateTime.UtcNow.AddHours(4);

            if (product.Files != null)
            {
                List<ProductImage> productImages = new List<ProductImage>();

                for (int i = 0; i < product.Files.Count; i++)
                {
                    if (!product.Files[i].CheckContentType("image/jpeg"))
                    {
                        ModelState.AddModelError("File", "You can choose only jpeg(jpg) format!");
                        return View();
                    }
                    if (product.Files[i].CheckFileLength(15000))
                    {
                        ModelState.AddModelError("File", "File must be 15MB at most!");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = await FileManager.CreateAsync(product.Files[i], _env, "assets", "images"),
                        ProductId = product.Id
                    };

                    if (i == 0)
                    {
                        product.MainImage = productImage.Image;
                    }
                    if (i == 1)
                    {
                        product.HoverImage = productImage.Image;
                    }

                    productImages.Add(productImage);
                }

                product.ProductImages = productImages;
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            TempData["success"] = "Product Is Created";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

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

            Product dbProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (dbProduct == null) return NotFound();

            if (await _context.Products.AnyAsync(p => p.ProductName.ToLower().Trim() == product.ProductName.ToLower().Trim() && !p.IsDeleted))
            {
                ModelState.AddModelError("Name", $"{product.ProductName} already exists");
                return View(product);
            }

            string brand = _context.Brands.FirstOrDefault(b => b.Id == product.BrandId).BrandName;
            string category = _context.Categories.FirstOrDefault(c => c.Id == product.CategoryId).Name;

            dbProduct.ProductName = product.ProductName.Trim();
            dbProduct.Seria = (brand.Substring(0, 2) + product.ProductName.Substring(0, 2)).ToLower();
            dbProduct.Code = No++;
            dbProduct.Price = product.Price;
            dbProduct.DiscountPrice = product.DiscountPrice;
            dbProduct.ExTax = product.ExTax;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.BrandId = product.BrandId;
            dbProduct.Count = product.Count;
            dbProduct.Description = product.Description;
            dbProduct.IsBestSeller = product.IsBestSeller;
            dbProduct.IsFeature = product.IsFeature;

            dbProduct.UpdatedAt = DateTime.UtcNow.AddHours(4);
            dbProduct.IsUpdated = true;

            //_context.Products.Update(dbProduct);
            //await _context.SaveChangesAsync();

            if (product.Files != null)
            {
                List<ProductImage> dbProductImages = await _context.ProductImages.Where(c => c.ProductId == product.Id).ToListAsync();

                for (int i = 0; i < dbProductImages.Count; i++)
                {
                    FileHelper.DeleteFile(_env, dbProductImages[i].Image, "assets", "images");
                    _context.ProductImages.Remove(dbProductImages[i]);
                    //await _context.SaveChangesAsync();
                }

                //main image ve hover image silmek lazimdi database den
                // icaze ermemek ki 2den az shekil olsun 
                // main ve hoveri deyishmek mecbudi olmasin
                //3 dene input mentigini gurmag
                //yoxlanishlar aparmag lazimdi ki update de hamsi mecburidi ya yox,
                //data annotationsa baxmag, burda update metodda error message elemek
                //delete product yazmag, shekilini silmemelidi prosto isDeleted true elemeleidi
                //evvel 10 shekil yuklesem create de sonra update edende 1 dene yuklesem, 10 shekili silir, databaseden, papkadan silir, amma productun 
                //main image ve hover imagesinnen silmir. onlari da silmek lazimdi burda, ve if qoymaq ki yuklenen shekiller minimum 2 dene olsun, seper

                List<ProductImage> productImages = new List<ProductImage>();

                for (int i = 0; i < product.Files.Count; i++)
                {
                    if (!product.Files[i].CheckContentType("image/jpeg"))
                    {
                        ModelState.AddModelError("File", "You can choose only jpeg(jpg) format!");
                        return View();
                    }

                    if (product.Files[i].CheckFileLength(15000))
                    {
                        ModelState.AddModelError("File", "File must be 15MB at most!");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = await FileManager.CreateAsync(product.Files[i], _env, "assets", "images"),
                        ProductId = dbProduct.Id
                    };

                    if (i == 0)
                    {
                        dbProduct.MainImage = productImage.Image;
                    }
                    if (i == 1)
                    {
                        dbProduct.HoverImage = productImage.Image;
                    }

                    productImages.Add(productImage);

                    //_context.Products.Update(dbProduct);
                    //_context.ProductImages.Update(productImage);
                    //await _context.SaveChangesAsync();
                }

                dbProduct.ProductImages = productImages;
            }

            await _context.SaveChangesAsync();

            TempData["success"] = "Product Is Updated!";

            return RedirectToAction("Index");
        }
    }
}
