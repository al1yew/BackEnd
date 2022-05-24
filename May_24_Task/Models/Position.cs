using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_24_Task.Models
{
    public class Position
    {
        public int Id { get; set; }
        public string PositionName { get; set; }
        public List<Trainer> Trainers { get; set; }
    }
}
