using May_24_Task.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_24_Task.Controllers
{
    public class TrainerController : Controller
    {
        private readonly AppDbContext _context;

        public TrainerController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //include delayem ptm shto bez nego on prinosit ix id, a nam nado ix imena. teper mi vkluchili v nash list positioni, i oni pridut v nash view.
            return View(_context.Trainers.Include(t => t.Position).ToList());
        }
    }
}
