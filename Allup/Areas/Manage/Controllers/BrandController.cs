using Allup.DAL;
using Allup.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;

        public BrandController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? status, int page = 1)
        {
            IQueryable<Brand> brands = _context.Brands;

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

            List<Brand> brandsList = await brands.Skip((page - 1) * brandCount).Take(brandCount).ToListAsync();
            //prosto bele gonderek, ozunu de View(bura yaza bilerik)
            ViewBag.Status = status;
            ViewBag.Page = page;
            ViewBag.BrandsCount = brandCount;
            ViewBag.PageCount = (int)Math.Ceiling((decimal)brands.Count() / brandCount);

            return View(brandsList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (await _context.Brands.AnyAsync(b => b.BrandName.ToLower().Trim() == brand.BrandName.ToLower().Trim() && !b.IsDeleted))
            {
                ModelState.AddModelError("Name", $"{brand.BrandName} already exists");
                return View();
            }

            brand.CreatedAt = DateTime.UtcNow.AddHours(+4);

            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();

            TempData["success"] = "Brand Is Created";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null) return NotFound();

            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Brand brand)
        {
            if (id == null) return BadRequest();

            if (id != brand.Id) return BadRequest();

            Brand dbBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == brand.Id);

            if (dbBrand == null) return NotFound();

            if (await _context.Brands.AnyAsync(b => b.Id != brand.Id && b.BrandName.ToLower().Trim() == brand.BrandName.ToLower().Trim() && !b.IsDeleted))
            {
                ModelState.AddModelError("Name", $"{brand.BrandName} already exists");
                return View();
            }

            dbBrand.BrandName = brand.BrandName;
            dbBrand.IsUpdated = true;
            dbBrand.UpdatedAt = DateTime.UtcNow.AddHours(+4);
            await _context.SaveChangesAsync();

            TempData["success"] = "Brand Is Updated";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id, int? status)
        {
            if (id == null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null) return NotFound();
            //if we want to delete permanently also from db we write:
            //_context.Brands.Remove(brand);

            brand.IsDeleted = true;
            brand.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            IQueryable<Brand> brands = _context.Brands;

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

            ViewBag.Status = status;

            return PartialView("_BrandIndexPartial", await brands.ToListAsync());
        }

        public async Task<IActionResult> Restore(int? id, int? status)
        {
            if (id == null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null) return NotFound();

            brand.IsDeleted = false;
            brand.DeletedAt = null;

            await _context.SaveChangesAsync();

            IQueryable<Brand> brands = _context.Brands;

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

            ViewBag.Status = status;

            return PartialView("_BrandIndexPartial", await brands.ToListAsync());
        }
    }
}
