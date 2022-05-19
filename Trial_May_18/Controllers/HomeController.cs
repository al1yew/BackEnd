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

        public readonly List<Group> _group;


        public HomeController()
        {
            _students = new List<Student>
            {
                new Student{ Id=1, Name = "Vasif", SurName = "Aliyev", Grade = 100 , GroupId = 3},
                new Student{ Id=2, Name = "Ali", SurName = "Mamedov", Grade = 52 , GroupId = 6},
                new Student{ Id=3, Name = "Alik", SurName = "Cabbarli", Grade = 63 , GroupId = 3},
                new Student{ Id=4, Name = "Idris", SurName = "Aliyev", Grade = 85 , GroupId = 2},
                new Student{ Id=5, Name = "Mamed", SurName = "Alidfsdyev", Grade = 100 , GroupId = 6},
                new Student{ Id=6, Name = "Isi", SurName = "Aliyev", Grade = 52 , GroupId = 6},
                new Student{ Id=7, Name = "Mercedes", SurName = "Aliyev", Grade = 25 , GroupId = 6},
                new Student{ Id=8, Name = "Vasif", SurName = "fda", Grade = 24 , GroupId = 7},
                new Student{ Id=9, Name = "Asif", SurName = "Aliydfsdev", Grade = 100 , GroupId = 6},
                new Student{ Id=10, Name = "Agasif", SurName = "Adfsaliyev", Grade = 52 , GroupId = 2},
                new Student{ Id=11, Name = "Vasif", SurName = "Aliyev", Grade = 56 , GroupId = 2},
            };
            //mi sozdali nashi listi i teper budem iskat chey id sovpadayet s id gruppi, i sobirat v ul li dla etoy gruppi.

            _group = new List<Group> {

                new Group{ Id = 10, Name = "t434", },
                new Group{ Id = 9, Name = "edDsdsas", },
                new Group{ Id = 8, Name = "eedfsdss", },
                new Group{ Id = 7, Name = "fd", },
                new Group{ Id = 6, Name = "edss", },
                new Group{ Id = 5, Name = "SDF", },
                new Group{ Id = 4, Name = "fads", },
                new Group{ Id = 3, Name = "edss", },
                new Group{ Id = 2, Name = "321", },
                new Group{ Id= 1, Name = "p321" },

            };
        }


        

        public IActionResult Index()
        {

            //return Json(_students.Find(x => x.Id == id));

            /*ViewBag.Name = "Mameeeeeeeed";
            ViewData["SurName"] = "Cavid";


            ViewBag.Students = _students;
*/
            //return RedirectToAction("Work");

            return View(_group);
            //indexegirende mene evvel gruplari gostersin indexin viewsunda
        }

        public IActionResult Student(int? id) {

            if (id == null)
            {
                return BadRequest();
            }
            else {
                List<Student> students = _students.FindAll(x => x.GroupId == id);
                //isteyirem ki mene hansisa id gelirse, gedim studentler siyahisinda butun hemin gruplarda oxuyanlari tapim getirim.
                //Niye mehz Find All cunki findall list qaytarir, cunki bir grupda bir telebe olmur axi, bir nce dene olur
                //Find metodu ise prosto telbebeni axtarsaq ishletmek duz olar, bir deyer qaytaranda
                return View(students);
            }
        }

    }
}
