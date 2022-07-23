using AutoMapper;
using FirstApi.Data.Entities;
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
            CreateMap<CategoryPostDto, Category>()
                .ForMember(des => des.Name, src => src.MapFrom(x => x.Name.Trim()))
                //.ForMember(des => des.Image, src => src.MapFrom(x => x.Image))
                //.ForMember(des => des.ParentId, src => src.MapFrom(x => x.ParentId))
                //.ForMember(des => des.IsMain, src => src.MapFrom(x => x.IsMain))
                .ForMember(des => des.CreatedAt, src => src.MapFrom(x => DateTime.UtcNow.AddHours(4)));
            //bildiyimiz category category = new category mentigidir, onu burda edirik, ve burda da ? operator ishlede
            //bilerik, ve burda da datetime veririk, null veririk, string ve saire sheyler vere bilerik

            CreateMap<Category, CategoryGetDto>();
            //    .ForMember(des => des.Name, src => src.MapFrom(x => x.Name.Trim()))
            //    .ForMember(des => des.Image, src => src.MapFrom(x => x.Image))
            //    .ForMember(des => des.ParentId, src => src.MapFrom(x => x.ParentId))
            //    .ForMember(des => des.IsMain, src => src.MapFrom(x => x.IsMain));

            //bulari elememek de olar cunki adlari eynidi, muelimde prop adlari ferqli idi,
            //yani prosto controllerde _mapper yazib source ve destination qeyd edirik, burda da neyi neye cevirir onu
            //amma .formember leri de yazsag hecne deyishmir.

            CreateMap<Category, CategoryListDto>();

            CreateMap<Category, CategoryGetDto>()
                .ForMember(des => des.ParentCategory, src => src.MapFrom(x => x.IsMain ? null : x.Parent.Name));
        }
    }
}
