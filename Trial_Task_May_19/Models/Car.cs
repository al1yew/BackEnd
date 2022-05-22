using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trial_Task_May_19.Models
{
    public class Car
    {
        //estestvenno u cara est property ved ya xochu otobrajat ego po dot notation

        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string EngineType { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        //bu property bizim relationu yaratdi, cunki car many terefdir, brand ise one terefdir, odin brend dejit mnogo variaciy modeley 
        //poetomu sdes mi otmechayem brand, tipa kakomu brendu budem otnositsa, a v Brand otmecayem shto u kajdogo brenda budet List modeley
        //eto nazivaem relation, i esli posmotrim v diagrammax v sql, tam est kluchik
    }
}
