﻿
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_23_HW.Controllers
{
    public class CourcesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
