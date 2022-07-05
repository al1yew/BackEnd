using Allup.DAL;
using Allup.Models;
using Allup.ViewModels.BasketViewModels;
using Allup.ViewModels.HeaderViewModels;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;

        public HeaderViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(IDictionary<string, string> settings)
        {
            string basket = HttpContext.Request.Cookies["basket"];

            List<BasketViewModel> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);

                if (User.Identity.IsAuthenticated)
                {
                    AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                    if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                    {
                        foreach (var item in appUser.Baskets)
                        {
                            if (!basketVMs.Any(b => b.ProductId == item.ProductId))
                            {
                                BasketViewModel basketVM = new BasketViewModel
                                {
                                    ProductId = item.ProductId,
                                    Count = item.Count
                                };

                                basketVMs.Add(basketVM);
                            }
                        }

                        basket = JsonConvert.SerializeObject(basketVMs);

                        HttpContext.Response.Cookies.Append("basket", basket);
                    }
                }

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
