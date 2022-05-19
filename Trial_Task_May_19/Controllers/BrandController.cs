using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trial_Task_May_19.Controllers
{
    public class BrandController : Controller
    {
        public IActionResult Index() {
            return View();
        }
    }
}
