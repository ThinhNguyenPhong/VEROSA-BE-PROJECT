using AutoMapper;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;
using VEROSA.DataAccessLayer.Entities;

namespace VEROSA_BE_PROJECT.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<ProductRequest, Product>().ReverseMap();
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ReverseMap();
        }
    }
}
