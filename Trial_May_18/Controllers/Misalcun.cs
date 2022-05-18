using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trial_May_18.Controllers
{
    public class Misalcun : Controller
    {
        public IActionResult Misal() { 
            return View("Misalcun");
        }
    }
}
