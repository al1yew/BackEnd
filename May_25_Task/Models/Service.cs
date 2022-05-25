using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_25_Task.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public int IconId { get; set; }
        public Icon Icon { get; set; }
        public string TextContent { get; set; }
    }
}
