using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trial_May_18.Models;

namespace Trial_May_18.Controllers
{
    public class HomeController : Controller
    {
        //public IActionResult Index() {
        //    return View("Index");
        //}

        private readonly List<Student> _students;

        public HomeController()
        {
            _students = new List<Student>
            {
                new Student{ Id=1, Name = "Vasif", SurName = "Aliyev", Grade = 100 },
                new Student{ Id=2, Name = "Ali", SurName = "Mamedov", Grade = 52 },
                new Student{ Id=3, Name = "Alik", SurName = "Cabbarli", Grade = 63 },
                new Student{ Id=4, Name = "Idris", SurName = "Aliyev", Grade = 85 },
                new Student{ Id=5, Name = "Mamed", SurName = "Alidfsdyev", Grade = 100 },
                new Student{ Id=6, Name = "Isi", SurName = "Aliyev", Grade = 52 },
                new Student{ Id=7, Name = "Mercedes", SurName = "Aliyev", Grade = 25 },
                new Student{ Id=8, Name = "Vasif", SurName = "fda", Grade = 24 },
                new Student{ Id=9, Name = "Asif", SurName = "Aliydfsdev", Grade = 100 },
                new Student{ Id=10, Name = "Agasif", SurName = "Adfsaliyev", Grade = 52 },
                new Student{ Id=11, Name = "Vasif", SurName = "Aliyev", Grade = 56 },
            };
        }

        public IActionResult Index(int? id)
        {

            //return Json(_students.Find(x => x.Id == id));

            ViewBag.Name = "Mameeeeeeeed";
            ViewData["SurName"] = "Cavid";


            ViewBag.Students = _students;

            //return RedirectToAction("Work");

            return View();
        }

        public IActionResult Work() {
            return View();
        }

    }
}
