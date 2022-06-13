using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.ViewModels.BasketViewModel
{
    public class BasketViewModel
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public double ExTax { get; set; }


    }
}
