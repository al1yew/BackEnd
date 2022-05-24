using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_24_HW.Models
{
    public class PricingPlan
    {
        public int Id { get; set; }
        public Pricing Pricing { get; set; }
        public int PricingId { get; set; }
        public int PlanId { get; set; }
        public Plan Plan { get; set; }
    }
}
