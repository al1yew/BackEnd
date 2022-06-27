using Allup.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Allup.Models;
using Allup.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Allup.Extensions;

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

        public async Task<IActionResult> Index(int? status, int page = 1)
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

            int itemCount = int.Parse(_context.Settings.FirstOrDefault(s => s.Key == "PageItemsCount").Value);

            ViewBag.Status = status;

            return View(PaginationList<Product>.Create(query, page, itemCount));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.BrandsForProducts = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.MainCategories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.BrandsForProducts = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.MainCategories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();

            if (!ModelState.IsValid) return View();

            if (await _context.Products.AnyAsync(p => p.ProductName.ToLower().Trim() == product.ProductName.ToLower().Trim() && !p.IsDeleted))
            {
                ModelState.AddModelError("Name", $"{product.ProductName} already exists");
                return View();
            }

            if (product.Files != null)
            {
                foreach (var item in product.Files)
                {
                    if (!item.CheckContentType("image/jpeg"))
                    {
                        ModelState.AddModelError("File", "You can choose only JPG(JPEG) format!");
                        return View();
                    }

                    if (item.CheckFileLength(15))
                    {
                        ModelState.AddModelError("File", "File must be 15kb at most!");
                        return View();
                    }

                }

            }

            product.CreatedAt = DateTime.UtcNow.AddHours(+4);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            TempData["success"] = "Product Is Created";

            return RedirectToAction("Index");

        }



    }
}
