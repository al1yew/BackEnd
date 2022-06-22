using Allup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.ViewModels.ProductViewModels
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; }
        public List<BrandSlider> BrandSliders { get; set; }
    }
}
