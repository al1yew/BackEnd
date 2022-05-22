
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trial_Task_May_19.Models
{
    public class Human
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PersonalCardId { get; set; } 
        public PersonalCard PersonalCard { get; set; }

        //her humanin ID si var, a ne naoborot. U kajfogo celvoeka id, ane u id est celovek
        //poetimu mi zovem sdes nash obyekt i obazatelno eshe k nemu hemin adnan Id stroka 13 i 14. Shto bi on svazalsa kluchom
        //s personal card klassom, i ordaki id-ni burdaki personalCardId ile == elesin
    }
}
