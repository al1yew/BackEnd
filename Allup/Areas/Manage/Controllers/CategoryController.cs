using Allup.DAL;
using Allup.Models;
using Allup.ViewModels;
using Microsoft.AspNetCore.Mvc;
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



    }
}
