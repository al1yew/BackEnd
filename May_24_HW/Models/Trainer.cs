using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_24_HW.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public Position Position { get; set; }
        public int PositionId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string TwitterUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string InstagramUrl { get; set; }
    }
}
