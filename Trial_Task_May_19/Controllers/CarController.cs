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

        //sonra  construktoru yigiram

        public CarController()
        {
            _cars = new List<Car> {
                new Car{ Id=1, Name = "e250", BrandId= 4, EngineType = "Manual", Year = 2020},
                new Car{ Id=2, Name = "e220", BrandId= 3, EngineType = "Manual", Year = 2015},
                new Car{ Id=3, Name = "e240", BrandId= 2, EngineType = "Manual", Year = 2010},
                new Car{ Id=4, Name = "e270", BrandId= 2, EngineType = "auto", Year = 2010},
                new Car{ Id=5, Name = "e280", BrandId= 2, EngineType = "Manual", Year = 2015},
                new Car{ Id=6, Name = "c270", BrandId= 2,  EngineType = "auto", Year = 2020},
                new Car{ Id=7, Name = "m340", BrandId= 2, EngineType = "Manual", Year = 2020},
                new Car{ Id=8, Name = "e25trgfd0", BrandId= 5, EngineType = "auto", Year = 2010},
                new Car{ Id=9, Name = "e25ed0", BrandId= 5, EngineType = "Manual", Year = 2018},
                new Car{ Id=10, Name = "e250dfs", BrandId= 3, EngineType = "Manual", Year = 2018},
                new Car{ Id=11, Name = "e252eg50", BrandId= 3, EngineType = "Manual", Year = 2020},
                new Car{ Id=12, Name = "vde25gr0", BrandId= 3, EngineType = "auto", Year = 2015},
                new Car{ Id=13, Name = "egrw250", BrandId= 8, EngineType = "auto", Year = 2018},
                new Car{ Id=14, Name = "e2eg0", BrandId= 7, EngineType = "auto", Year = 2015},
                new Car{ Id=15, Name = "e25ewgw", BrandId= 3, EngineType = "Manual", Year = 2020},
                new Car{ Id=16, Name = "e2we0", BrandId= 3, EngineType = "Manual", Year = 2010},
                new Car{ Id=17, Name = "e23r0", BrandId= 1, EngineType = "Manual", Year = 2015},
                new Car{ Id=18, Name = "e23r0", BrandId= 1, EngineType = "Manual", Year = 2015},
                new Car{ Id=19, Name = "e23r0", BrandId= 1, EngineType = "Manual", Year = 2015},
                new Car{ Id=20, Name = "e23r0", BrandId= 1, EngineType = "Manual", Year = 2015},
                new Car{ Id=21, Name = "e23r0", BrandId= 4, EngineType = "Manual", Year = 2015},
                new Car{ Id=22, Name = "e23r0", BrandId= 4, EngineType = "Manual", Year = 2015},
                new Car{ Id=23, Name = "e23r0", BrandId= 4, EngineType = "Manual", Year = 2015},
                new Car{ Id=24, Name = "e23r0", BrandId= 6, EngineType = "Manual", Year = 2015},
                new Car{ Id=25, Name = "e23r0", BrandId= 6, EngineType = "Manual", Year = 2015},
                new Car{ Id=26, Name = "e23r0", BrandId= 6, EngineType = "Manual", Year = 2015},
                new Car{ Id=27, Name = "e23r0", BrandId= 6, EngineType = "Manual", Year = 2015},
                new Car{ Id=28, Name = "e23r0", BrandId= 6, EngineType = "Manual", Year = 2015},
            };
        }

        public IActionResult Index(int? id)
        {
            //mene axi id gelecek branda basanda, on prineset mne id i ya suda vstavlu ego, i vernu emu hemin id-de duran elementleri
            //pcm vopros? ptm shto mojet pridti, a mojet i net
            //eto tot je id kotoriy v startape
            return View(_cars);
            //estesna vernet mne view i idu v ego view
            //ya otpravlayu tuda v index v car v view svoy list _cars
        }
    }
}
