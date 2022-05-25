using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_25_Task.Models
{
    public class Position
    {
        public int Id { get; set; }
        public List<Team> Teams { get; set; }
        public string PositionName { get; set; }
    }
}
