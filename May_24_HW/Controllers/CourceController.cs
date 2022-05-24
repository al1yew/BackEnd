﻿using May_24_HW.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_24_HW.Controllers
{
    public class CourceController : Controller
    {
        private readonly AppDbContext _context;

        public CourceController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //_context.Cources.Include(c => c.)
            return View(_context.Cources.ToList());
        }
    }
}
