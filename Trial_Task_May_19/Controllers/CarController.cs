using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trial_Task_May_19.Models;

namespace Trial_Task_May_19.Controllers
{
    public class CarController : Controller
    {
        private readonly List<Car> _cars;
        //carsin modelini yaratdim

        public IActionResult Index(int? id) {
            //mene axi id gelecek branda basanda, on prineset mne id i ya suda vstavlu ego, i vernu emu hemin id-de duran elementleri
            //pcm vopros? ptm shto mojet pridti, a mojet i net
            //eto tot je id kotoriy v startape
            return View();
            //estesna vernet mne view i idu v ego view
        }

        //sonra  construktoru yigiram

        public CarController()
        {
            _cars = new List<Car> { 
                new Car{ Id=1, Name = "e250", BrandId=}
            };
        }
    }
}
    