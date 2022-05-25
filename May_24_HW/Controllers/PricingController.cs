using May_24_HW.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using May_24_HW.PricingViewModels;

namespace May_24_HW.Controllers
{
    public class PricingController : Controller
    {
        private readonly AppDbContext _context;

        public PricingController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            PricingViewModel pricingViewModel = new PricingViewModel
            {
                Pricings = _context.Pricings.Include(p => p.PricingPlans).ToList(),
                Plans = _context.Plans.ToList()
            };

            return View(pricingViewModel);

        }
    }
}
