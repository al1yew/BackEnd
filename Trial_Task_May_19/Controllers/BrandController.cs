using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        }

        public IActionResult Index()
        {
            //List<Brand> brands = _context.Brands.ToList();
            //u nas est INCLUDE. on pomogayet nam rabotat bez foreach, toest mi po odnimu vse brandi krutit i ix modeli sobirat ne budem
            //mi include sdelayem tot kotkriy nujen
            //snizu pishu to je samoe shto sverhu, no uje inkludom

            List<Brand> brands = _context.Brands.Include(b=> b.Cars).ToList();

            //toest idi prinesi mne brendi iz databazi, i her birinin icine include ele onun Cars listini, Ve hamsini yig brend listine

            Car car = _context.Cars.Include(c => c.Brand).FirstOrDefault(t => t.Id == 2);

            return View(car);
            //ctrl f5 nese ishlemedi amma neyse

        }
    }
}
