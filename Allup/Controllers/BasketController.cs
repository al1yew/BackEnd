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
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
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
                    Count = 1
                };

                BasketVMs.Add(basketViewModel);
            }

            basket = JsonConvert.SerializeObject(BasketVMs);

            HttpContext.Response.Cookies.Append("basket", basket);

            return PartialView("_BasketPartial", await _getBasketItemAsync(BasketVMs));
        }

        public async Task<IActionResult> DeleteFromBasket(int? id)
        {
            if (id == null) return BadRequest();

            if (!await _context.Products.AnyAsync(p => p.Id == id)) return NotFound();

            string basket = HttpContext.Request.Cookies["basket"];

            if (string.IsNullOrWhiteSpace(basket)) return BadRequest();

            List<BasketViewModel> BasketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);

            BasketViewModel basketVM = BasketVMs.Find(b => b.ProductId == id);

            if (BasketVMs == null) return NotFound();

            BasketVMs.Remove(basketVM);

            basket = JsonConvert.SerializeObject(BasketVMs);

            HttpContext.Response.Cookies.Append("basket", basket);

            return PartialView("_BasketPartial", await _getBasketItemAsync(BasketVMs));

        }

        private async Task<List<BasketViewModel>> _getBasketItemAsync(List<BasketViewModel> basketVms)
        {
            foreach (BasketViewModel item in basketVms)
            {
                Product dbProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);

                item.Name = dbProduct.ProductName;
                item.Price = dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.Price;
                item.ExTax = dbProduct.ExTax;
                item.Image = dbProduct.MainImage;

            }

            return basketVms;
        }
    }
}
