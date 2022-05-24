using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_24_HW.Controllers
{
    public class CourceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
