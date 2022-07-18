using Allup.DAL;
using Allup.Models;
using Allup.ViewModels.BasketViewModels;
using Allup.ViewModels.HomeViewModels;
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
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products.ToListAsync();

            HomeViewModel homeViewModel = new HomeViewModel
            {
                Products = await _context.Products.ToListAsync(),
                Sliders = await _context.Sliders.ToListAsync(),
                BestSeller = products.Where(p => p.IsBestSeller).ToList(),
                Feature = products.Where(p => p.IsFeature).ToList(),
                NewArrival = products.Where(p => p.IsNewArrival).ToList()
            };

            return View(homeViewModel);
        }

        public async Task<IActionResult> Chat()
        {
            List<AppUser> appUsers = await _userManager.Users.Where(u => !u.IsAdmin).ToListAsync();

            return View(appUsers);
        }
    }
}