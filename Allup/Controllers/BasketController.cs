using Allup.DAL;
using Allup.Models;
using Allup.ViewModels.BasketViewModels;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;

        public BasketController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            string basket = HttpContext.Request.Cookies["basket"];

            List<BasketViewModel> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            }
            else
            {
                basketVMs = new List<BasketViewModel>();
            }

            return View(await _getBasketItemAsync(basketVMs));
        }

        public async Task<IActionResult> AddToBasket(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            string basket = HttpContext.Request.Cookies["basket"];

            List<BasketViewModel> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            }
            else
            {
                basketVMs = new List<BasketViewModel>();
            }

            if (basketVMs.Exists(bvm => bvm.ProductId == id))
            {
                basketVMs.Find(bvm => bvm.ProductId == id).Count++;
            }
            else
            {
                BasketViewModel basketViewModel = new BasketViewModel
                {
                    ProductId = product.Id,
                    Count = 1
                };

                basketVMs.Add(basketViewModel);
            }

            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                {
                    Basket dbBasket = appUser.Baskets.FirstOrDefault(b => b.ProductId == id);

                    if (dbBasket != null)
                    {
                        dbBasket.Count += 1;
                    }
                    else
                    {
                        Basket newBasket = new Basket
                        {
                            ProductId = (int)id,
                            Count = 1
                        };

                        appUser.Baskets.Add(newBasket);
                    }
                }
                else
                {
                    List<Basket> baskets = new List<Basket>
                    {
                        new Basket{ProductId = (int)id, Count = 1}
                    };

                    appUser.Baskets = baskets;
                }

                await _context.SaveChangesAsync();
            }

            basket = JsonConvert.SerializeObject(basketVMs);

            HttpContext.Response.Cookies.Append("basket", basket);

            return PartialView("_BasketPartial", await _getBasketItemAsync(basketVMs));
        }

        public async Task<IActionResult> UpdateCount(int? id, int count)
        {
            if (id == null) return BadRequest();

            if (!await _context.Products.AnyAsync(p => p.Id == id)) return NotFound();

            string basket = HttpContext.Request.Cookies["basket"];

            List<BasketViewModel> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);

                BasketViewModel basketVM = basketVMs.FirstOrDefault(b => b.ProductId == id);

                if (basketVM == null) return NotFound();

                basketVM.Count = count <= 0 ? 1 : count;

                basket = JsonConvert.SerializeObject(basketVMs);

                HttpContext.Response.Cookies.Append("basket", basket);
            }
            else
            {
                return BadRequest();
            }

            return PartialView("_BasketIndexPartial", await _getBasketItemAsync(basketVMs));
        }

        public async Task<IActionResult> DeleteFromCart(int? id)
        {
            if (id == null) return BadRequest();

            if (!await _context.Products.AnyAsync(p => p.Id == id)) return NotFound();

            string basket = HttpContext.Request.Cookies["basket"];

            if (string.IsNullOrWhiteSpace(basket)) return BadRequest();

            List<BasketViewModel> basketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);

            BasketViewModel basketVM = basketVMs.Find(b => b.ProductId == id);

            if (basketVM == null) return NotFound();

            basketVMs.Remove(basketVM);

            basket = JsonConvert.SerializeObject(basketVMs);

            HttpContext.Response.Cookies.Append("basket", basket);

            return PartialView("_BasketIndexPartial", await _getBasketItemAsync(basketVMs));
        }

        public async Task<IActionResult> DeleteFromBasket(int? id)
        {
            if (id == null) return BadRequest();

            if (!await _context.Products.AnyAsync(p => p.Id == id)) return NotFound();

            string basket = HttpContext.Request.Cookies["basket"];

            if (string.IsNullOrWhiteSpace(basket)) return BadRequest();

            List<BasketViewModel> BasketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);

            BasketViewModel basketVM = BasketVMs.Find(b => b.ProductId == id);

            if (basketVM == null) return NotFound();

            BasketVMs.Remove(basketVM);

            basket = JsonConvert.SerializeObject(BasketVMs);

            HttpContext.Response.Cookies.Append("basket", basket);

            return PartialView("_BasketPartial", await _getBasketItemAsync(BasketVMs));
        }

        private async Task<List<BasketViewModel>> _getBasketItemAsync(List<BasketViewModel> basketVms)
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            }

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

        public async Task<IActionResult> GetBasket()
        {
            string basket = HttpContext.Request.Cookies["basket"];

            List<BasketViewModel> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            }
            else
            {
                basketVMs = new List<BasketViewModel>();
            }

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

                    Response.Cookies.Append("basket", basket);
                }
            }

            return PartialView("_BasketPartial", await _getBasketItemAsync(basketVMs));
        }
    }
}
