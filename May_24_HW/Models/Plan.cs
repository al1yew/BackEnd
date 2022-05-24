using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_24_HW.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PricingPlan> PricingPlans { get; set; }
    }
}
