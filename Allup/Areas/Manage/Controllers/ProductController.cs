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
            #region trial
            // create doljen prinimat productDTO i vmesto product nado pisat productdto
            //vse shto posle regiona napisano posle togo kak region sozdan
            //int code = 1;
            //string brand = _context.Brands.FirstOrDefault(b => b.Id == productdto.BrandId).BrandName;
            //string category = _context.Categories.FirstOrDefault(c => c.Id == productdto.CategoryId).Name;

            //Product product = new Product
            //{
            //    ProductName = productdto.ProductName,
            //    Price = productdto.Price,
            //    DiscountPrice = productdto.DiscountPrice,
            //    ExTax = productdto.ExTax,
            //    Seria = brand.Substring(0, 2) + productdto.ProductName.Substring(0, 2),
            //    Code = code,
            //    Description = productdto.Description,
            //    Count = productdto.Count,

            //    BrandId = productdto.BrandId,
            //    CategoryId = productdto.CategoryId,

            //    MainImage = productdto.Files[0].FileName,
            //    HoverImage = productdto.Files[1].FileName,
            //    IsBestSeller = productdto.IsBestSeller,
            //    IsFeature = productdto.IsFeature
            //};

            //code++;

            //if (productdto.Files != null)
            //{
            //    foreach (var item in productdto.Files)
            //    {
            //        if (!item.CheckContentType("image/jpeg"))
            //        {
            //            ModelState.AddModelError("File", "You can choose only JPG(JPEG) format!");
            //            return View();
            //        }

            //        if (item.CheckFileLength(15000))
            //        {
            //            ModelState.AddModelError("File", "File must be 15000kb at most!");
            //            return View();
            //        }

            //        ProductImage productImage = new ProductImage
            //        {
            //            Image = await FileManager.CreateAsync(item, _env, "assets", "images"),
            //            ProductId= product.Id
            //        };
            //    }
            //}
            #endregion

            string brand = _context.Brands.FirstOrDefault(b => b.Id == product.BrandId).BrandName;
            string category = _context.Categories.FirstOrDefault(c => c.Id == product.CategoryId).Name;

            product.ProductName = product.ProductName.Trim();
            product.Seria = (brand.Substring(0, 2) + product.ProductName.Substring(0, 2)).ToLower();
            product.Code = 1;
            product.IsNewArrival = true;
            product.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            #region foreach photos

            //if (product.Files != null)
            //{
            //    foreach (var item in product.Files)
            //    {
            //        if (!item.CheckContentType("image/jpeg"))
            //        {
            //            ModelState.AddModelError("File", "You can choose only JPG(JPEG) format!");
            //            return View();
            //        }
            //        if (item.CheckFileLength(15000))
            //        {
            //            ModelState.AddModelError("File", "File must be 15000kb at most!");
            //            return View();
            //        }

            //        ProductImage productImage = new ProductImage
            //        {
            //            Image = await FileManager.CreateAsync(item, _env, "assets", "images"),
            //            ProductId = product.Id
            //        };

            //        await _context.ProductImages.AddAsync(productImage);
            //        await _context.SaveChangesAsync();
            //    }
            //}
            #endregion

            #region For photos

            if (product.Files != null)
            {
                for (int i = 0; i < product.Files.Count; i++)
                {
                    if (!product.Files[i].CheckContentType("image/jpeg"))
                    {
                        ModelState.AddModelError("File", "You can choose only JPG(JPEG) format!");
                        return View();
                    }
                    if (product.Files[i].CheckFileLength(15000))
                    {
                        ModelState.AddModelError("File", "File must be 15000kb at most!");
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

                    _context.Products.Update(product);
                    await _context.ProductImages.AddAsync(productImage);
                    await _context.SaveChangesAsync();
                }
            }
            #endregion

            TempData["success"] = "Product Is Created";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id) 
        { 
            return View();
        }
    }
}
