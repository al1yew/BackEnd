using May_25_Task.DAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_25_Task.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context
            )
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Contacts.ToList());
        }
    }
}
