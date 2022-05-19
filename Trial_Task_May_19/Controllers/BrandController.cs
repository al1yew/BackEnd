using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trial_Task_May_19.Models;

namespace Trial_Task_May_19.Controllers
{
    public class BrandController : Controller
    {
        //shto bi sozdat sdes shto to u Branda doljni bit svoi property, poetomu nado ix sozdat v Brand class

        //teper property zadani, mogu sozdat sdes list

        private readonly List<Brand> _brands;

        //prosle togo kak ya sozdal list nado pozvat konstruktor 

        public BrandController()
        {
            //on zovetsa avtomaticeski po imeni kontrollera
            _brands = new List<Brand> {
                new Brand {Id = 1, Name = "Mercedes" },
                new Brand {Id = 2, Name = "Opel" },
                new Brand {Id = 3, Name = "BMW" },
                new Brand {Id = 4, Name = "Kia" },
                new Brand {Id = 5, Name = "Hyundai" },
                new Brand {Id = 6, Name = "Jaguar" },
                new Brand {Id = 7, Name = "Land Rover" },
            };
            //sozdal svoy list, zapomni sintaksis, teper on gotov i ya mogu poslat ego v svoy view, a on razlojit ego po ul li
        }

        public IActionResult Index()
        {
            return View(_brands);
        }
    }
}
