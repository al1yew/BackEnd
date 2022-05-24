using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_24_HW.Models
{
    public class Pricing
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsAdvanced { get; set; }
        List<PricingPlan> PricingPlans { get; set; }
    }
}
