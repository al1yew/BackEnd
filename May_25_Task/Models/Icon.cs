using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_25_Task.Models
{
    public class Icon
    {
        public int Id { get; set; }
        public List<Service> Services { get; set; }
        public string IconClass { get; set; }
    }
}
