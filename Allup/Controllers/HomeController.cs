using Allup.DAL;
using Allup.Models;
using Allup.ViewModels.BasketViewModels;
using Allup.ViewModels.HomeViewModels;
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
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
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
    }
}