using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using May_24_HW.Models;

namespace May_24_HW.PricingViewModels
{
    public class PricingViewModel
    {
        public List<Pricing> Pricings { get; set; }
        public List<Plan> Plans { get; set; }
    }
}
