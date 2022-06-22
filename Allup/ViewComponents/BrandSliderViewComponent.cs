using Allup.DAL;
using Allup.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.ViewComponents
{
    public class BrandSliderViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public BrandSliderViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(List<BrandSlider> brandSliders) 
        {
            return View(await Task.FromResult(brandSliders));
        }
    }
}
