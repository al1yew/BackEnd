using Allup.DAL;
using Allup.Models;
using Allup.ViewModels.BasketViewModels;
using Allup.ViewModels.HeaderViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public HeaderViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(IDictionary<string, string> settings)
        {
            string basket = HttpContext.Request.Cookies["basket"];

            List<BasketViewModel> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);

                foreach (BasketViewModel basketVM in basketVMs)
                {
                    Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketVM.ProductId);

                    basketVM.Image = product.MainImage;
                    basketVM.Name = product.ProductName;
                    basketVM.ExTax = product.ExTax;
                    basketVM.Price = product.DiscountPrice > 0 ? product.DiscountPrice : product.Price;
                }
            }
            else
            {
                basketVMs = new List<BasketViewModel>();
            }

            HeaderViewModel headerViewModel = new HeaderViewModel
            {
                Settings = settings,
                BasketVMs = basketVMs
            };

            return View(await Task.FromResult(headerViewModel));
        }
    }
}
