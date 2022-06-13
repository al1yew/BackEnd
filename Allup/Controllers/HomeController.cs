using Allup.DAL;
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
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            string basket = HttpContext.Request.Cookies["basket"];

            List<BasketViewModel> basketVMs = null;

            if (basket != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            }
            else
            {
                basketVMs = new List<BasketViewModel>();
            }

            return View(await _context.Products.ToListAsync());
        }
    }
}
