using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trial_Task_May_19.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public List<Car> Cars { get; set; }
    }
}
