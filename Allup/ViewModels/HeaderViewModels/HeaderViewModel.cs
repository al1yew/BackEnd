using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Allup.ViewModels.BasketViewModels;

namespace Allup.ViewModels.HeaderViewModels
{
    public class HeaderViewModel
    {
        public IDictionary<string, string> Settings { get; set; }
        public List<BasketViewModel> BasketVMs { get; set; }
    }
}
