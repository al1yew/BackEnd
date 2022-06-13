using Allup.DAL;
using Allup.Models;
using Allup.ViewModels.BasketViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
            //return View(await _context.Products
            //    .Include(p => p.ProductToColors).ThenInclude(pc => pc.Color)
            //    .Include(p => p.ProductToSizes).ThenInclude(ps => ps.Size)
            //    .ToListAsync());
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = await _context.Products
                .Include(p => p.ProductToTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductImages)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> DetailModal(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = await _context.Products
                .Include(p => p.ProductToTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductImages)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return PartialView("_ProductModalPartial", product);
        }

        public async Task<IActionResult> Search(string search)
        {
            List<Product> products = await _context.Products
                .Where(p => p.ProductName.ToLower().Contains(search.ToLower()) ||
                p.Brand.BrandName.ToLower().Contains(search.ToLower()) ||
                p.ProductToTags.Any(pt => pt.Tag.TagName.ToLower().Contains(search.ToLower()))).ToListAsync();

            return PartialView("_SearchPartial", products);
        }

        public async Task<IActionResult> AddToBasket(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            string basket = HttpContext.Request.Cookies["basket"];

            List<BasketViewModel> BasketVMs = null;

            if (basket != null)
            {
                BasketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            }
            else
            {
                BasketVMs = new List<BasketViewModel>();
            }

            if (BasketVMs.Exists(bvm => bvm.ProductId == id))
            {
                BasketVMs.Find(bvm => bvm.ProductId == id).Count++;
            }
            else
            {
                BasketViewModel basketViewModel = new BasketViewModel
                {
                    ProductId = product.Id,
                    Count = 1,
                    Image = product.MainImage,
                    Name = product.ProductName,
                    Price = product.DiscountPrice > 0 ? product.DiscountPrice : product.Price,
                    ExTax = product.ExTax
                };

                BasketVMs.Add(basketViewModel);
            }

            basket = JsonConvert.SerializeObject(BasketVMs);

            HttpContext.Response.Cookies.Append("basket", basket);

            return PartialView("_BasketPartial", BasketVMs);
        }
    }
}
