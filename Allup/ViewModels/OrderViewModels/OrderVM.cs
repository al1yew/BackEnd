using Allup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.ViewModels.OrderViewModels
{
    public class OrderVM
    {
        public Order Order { get; set; }
        public List<Basket> Baskets { get; set; }
    }
}
