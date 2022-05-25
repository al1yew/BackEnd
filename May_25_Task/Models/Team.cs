using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_25_Task.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
        public string TextContent { get; set; }
        public string TwitterUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string IntagramUrl { get; set; }

    }
}
