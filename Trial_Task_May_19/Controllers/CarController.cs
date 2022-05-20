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

            List<Car> findingList = null;

            if (id != null)
            {
                //eyni yazilishdi no Where eto metod dla entity framework, i v konce obazatelno nado sdelat tolsit ved nam prixodit ne kak list
                findingList = _context.Cars.Where(x => x.BrandId == id).ToList();
            }
            else
            {
                findingList = _context.Cars.ToList();
            }


            return View(findingList);
        }

        public IActionResult Detail(int? id)
        {

            return View(_context.Cars.FirstOrDefault(c => c.Id == id));

            //first or default eto tipa find metod lda lista, a where eto tipa findall metod. tut ya tolist ne delayu ved u mena
            //1 data, list ne nujen
        }
    }
}
