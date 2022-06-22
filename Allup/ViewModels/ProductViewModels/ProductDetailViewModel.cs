using Allup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.ViewModels.ProductViewModels
{
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public List<BrandSlider> BrandSliders { get; set; }
    }
}
