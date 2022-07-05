using Allup.DAL;
using Allup.Models;
using Allup.ViewModels.AccountViewModels;
using Allup.ViewModels.BasketViewModels;
using Microsoft.AspNetCore.Authorization;
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
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            AppDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser appUser = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.SurName,
                UserName = registerVM.UserName,
                Email = registerVM.Email
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            result = await _userManager.AddToRoleAsync(appUser, "Member");

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.Email);
            //mojno eshe po username sdelat shto bi on naxodil ne tolko po emailu no i po userneymu

            if (appUser == null)
            {
                ModelState.AddModelError("", "Email or Password is wrong!");
                return View(loginVM);
            }

            if (appUser.IsAdmin)
            {
                ModelState.AddModelError("", "Email Or Password Is InCorrect");
                return View(loginVM);
            }

            if (!await _userManager.CheckPasswordAsync(appUser, loginVM.Password))
            {
                ModelState.AddModelError("", "Email or Password is wrong!");
                return View(loginVM);
            }

            await _signInManager.SignInAsync(appUser, loginVM.RememberMe);

            string basketCookie = HttpContext.Request.Cookies["basket"];

            if (!string.IsNullOrWhiteSpace(basketCookie))
            {
                List<BasketViewModel> BasketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketCookie);

                List<Basket> baskets = new List<Basket>();

                foreach (BasketViewModel BasketViewModel in BasketViewModels)
                {
                    if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                    {
                        Basket existedBasket = appUser.Baskets.FirstOrDefault(b => b.ProductId != BasketViewModel.ProductId);

                        if (existedBasket == null)
                        {
                            Basket basket = new Basket
                            {
                                AppUserId = appUser.Id,
                                ProductId = BasketViewModel.ProductId,
                                Count = BasketViewModel.Count
                            };

                            baskets.Add(basket);
                        }
                        else
                        {
                            existedBasket.Count += BasketViewModel.Count;
                            BasketViewModel.Count = existedBasket.Count;
                        }
                    }
                    else
                    {
                        Basket basket = new Basket
                        {
                            AppUserId = appUser.Id,
                            ProductId = BasketViewModel.ProductId,
                            Count = BasketViewModel.Count
                        };

                        baskets.Add(basket);
                    }
                }

                basketCookie = JsonConvert.SerializeObject(BasketViewModels);

                HttpContext.Response.Cookies.Append("basket", basketCookie);

                await _context.Baskets.AddRangeAsync(baskets);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                {
                    List<BasketViewModel> BasketViewModels = new List<BasketViewModel>();

                    foreach (Basket basket in appUser.Baskets)
                    {
                        BasketViewModel BasketViewModel = new BasketViewModel
                        {
                            ProductId = basket.ProductId,
                            Count = basket.Count
                        };

                        BasketViewModels.Add(BasketViewModel);
                    }

                    basketCookie = JsonConvert.SerializeObject(BasketViewModels);

                    HttpContext.Response.Cookies.Append("basket", basketCookie);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        #region Created Roles

        //public async Task<IActionResult> CreateRole()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });

        //    return Content("Salammmmm");
        //}

        #endregion

        #region Created Super Admin

        //public async Task<IActionResult> CreateSuperAdmin()
        //{
        //    AppUser appUser = new AppUser
        //    {
        //        Name = "Super",
        //        Surname = "Admin",
        //        UserName = "SuperAdmin",
        //        Email = "vasifja@code.edu.az"
        //    };

        //    appUser.IsAdmin = true;

        //    await _userManager.CreateAsync(appUser, "SuperAdmin@12345");

        //    await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

        //    return Content("Super admin est ");
        //}
        #endregion
    }
}
