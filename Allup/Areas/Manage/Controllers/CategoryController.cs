using Allup.DAL;
using Allup.Models;
using Allup.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? status, int page = 1)
        {
            IQueryable<Category> brands = _context.Categories;

            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    brands = brands.Where(b => b.IsDeleted);
                }
                else if (status == 2)
                {
                    brands = brands.Where(b => !b.IsDeleted);
                }
            }

            int brandCount = int.Parse(_context.Settings.FirstOrDefault(s => s.Key == "PageBrandsCount").Value);

            ViewBag.Status = status;

            return View(PaginationList<Category>.Create(brands, page, brandCount));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.MainCategories = await _context.Categories.Where(c => !c.IsDeleted && c.IsMain).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            ViewBag.MainCategories = await _context.Categories.Where(c => !c.IsDeleted && c.IsMain).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (category.IsMain && category.Image == null)
            {
                ModelState.AddModelError("Image", "Image is required in main category!");
                return View();
            }
            else
            {
                if (category.ParentId == null || !await _context.Categories.AnyAsync(c => !c.IsDeleted && c.IsMain && c.Id == category.ParentId))
                {
                    ModelState.AddModelError("ParentId", "Parent Category is chosen incorrect!");
                    return View();
                }
            }

            if (await _context.Categories.AnyAsync(c => !c.IsDeleted && c.Name == category.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "Already Exists");
                return View();
            }

            category.Name = category.Name.Trim();
            category.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
