using AutoMapper;
using FirstApi.Data.Entities;
using FirstApi.DTOs.BrandDTOs;
using FirstApi.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //--------------------------Categories

            CreateMap<CategoryPostDto, Category>()
                .ForMember(des => des.Name, src => src.MapFrom(x => x.Name.Trim()))
                .ForMember(des => des.CreatedAt, src => src.MapFrom(x => DateTime.UtcNow.AddHours(4)));

            CreateMap<Category, CategoryGetDto>();

            CreateMap<Category, CategoryListDto>();
            //perviy eto source, vtoroy eto destination
            CreateMap<Category, CategoryGetDto>()
                .ForMember(des => des.ParentCategory, src => src.MapFrom(x => x.IsMain ? null : x.Parent.Name));
            //bunu ozumcun yoxladim amma bunsuz da olar, yuxarida bele create map var onsuzda, misalcun brandda bunu yazmiram

            //--------------------------Brands

            CreateMap<Brand, BrandListDto>();

            CreateMap<Brand, BrandGetDto>();

            CreateMap<BrandPostDto, Brand>();

        }
    }
}
