using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_25_Task.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public string PlanType { get; set; }
        public List<PricingPlan> PricingPlans { get; set; }
    }
}
