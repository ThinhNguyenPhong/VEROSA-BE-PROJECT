using AutoMapper;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;
using VEROSA.DataAccessLayer.Entities;

namespace VEROSA_BE_PROJECT.Mappers
{
    public class ProductCategoryMapper : Profile
    {
        public ProductCategoryMapper()
        {
            CreateMap<ProductCategoryRequest, ProductCategory>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryResponse>().ReverseMap();
        }
    }
}
