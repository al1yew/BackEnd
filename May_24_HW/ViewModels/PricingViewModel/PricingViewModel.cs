using May_24_HW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_24_HW.ViewModels.PricingViewModel
{
    public class PricingViewModel
    {
        public List<Pricing> Pricings { get; set; }
        public List<Plan> Plans { get; set; }
    }
}
