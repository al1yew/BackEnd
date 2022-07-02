using Allup.Models;
using Allup.ViewModels.BasketViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Interfaces
{
    public interface ILayoutService
    {
        Task<List<BasketViewModel>> GetBasket();
        Task<IDictionary<string, string>> GetSetting();
        Task<List<BrandSlider>> GetBrandSlider();
        Task<List<Testimonial>> GetTestimonial();
        Task<List<Feature>> GetFeature();
    }
}
