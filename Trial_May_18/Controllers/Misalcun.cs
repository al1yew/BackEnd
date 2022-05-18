using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trial_May_18.Controllers
{
    public class Misalcun : Controller
    {
        private string[] str_arr = {"asdasd", "hello", "privet", "good morning", "greetings", "hola", "salam"};

        public IActionResult Misal(int id) {
            //return View("Misalcun");
            return Content(str_arr[id]);
            //mene arrayin id-incisini qaytarir, prosto domaine yazmaq lazimdi /misalcun/misal/regem kotoriy id

        }
    }
}
