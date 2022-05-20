using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trial_Task_May_19.DAL;
using Trial_Task_May_19.Models;

namespace Trial_Task_May_19.Controllers
{
    public class BrandController : Controller
    {
        //private readonly List<Brand> _brands;
        private readonly AppDbContext _context;

        public BrandController(AppDbContext context)
        {

            _context = context;


            //_brands = new List<Brand> {
            //    new Brand {Id = 1, Name = "Mercedes" },
            //    new Brand {Id = 2, Name = "Opel" },
            //    new Brand {Id = 3, Name = "BMW" },
            //    new Brand {Id = 4, Name = "Kia" },
            //    new Brand {Id = 5, Name = "Hyundai" },
            //    new Brand {Id = 6, Name = "Jaguar" },
            //    new Brand {Id = 7, Name = "Land Rover" },
            //};
        }

        public IActionResult Index()
        {
            List<Brand> brands = _context.Brands.ToList();

            return View(brands);

        }
    }
}
