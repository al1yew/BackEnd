using Allup.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.ViewComponents
{
    public class NewsLetterViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public NewsLetterViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(IDictionary<string, string> settings)
        {
            return View(await Task.FromResult(settings));
        }
    }
}
