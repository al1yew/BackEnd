using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_24_HW.Models
{
    public class Cource
    {
        public int Id { get; set; }
        public string  Image { get; set; }
        public string Type { get; set; }
        public string Price { get; set; }
        public string Header { get; set; }
        public string TextContent { get; set; }
        public string SmallImage { get; set; }
        public int Liked { get; set; }
        public int Users { get; set; }
        public string Name { get; set; }
    }
}
