﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Controllers
{
    public class CompareController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}