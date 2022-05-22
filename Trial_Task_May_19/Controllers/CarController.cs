using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trial_Task_May_19.DAL;
using Trial_Task_May_19.Models;

namespace Trial_Task_May_19.Controllers
{
    public class CarController : Controller
    {

        private readonly AppDbContext _context;

        public CarController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? id)
        {
            return View();

        }

        public IActionResult Detail(int? id)
        {
            return View();

        }
    }
}
