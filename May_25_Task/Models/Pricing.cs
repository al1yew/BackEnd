using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_25_Task.Models
{
    public class Pricing
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
        public bool IsFeatured { get; set; }
        public List<PricingPlan> PricingPlans { get; set; }
    }
}
