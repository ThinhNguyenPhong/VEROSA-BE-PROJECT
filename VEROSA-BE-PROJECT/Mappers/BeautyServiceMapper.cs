using AutoMapper;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;
using VEROSA.DataAccessLayer.Entities;

namespace VEROSA_BE_PROJECT.Mappers
{
    public class BeautyServiceMapper : Profile
    {
        public BeautyServiceMapper()
        {
            CreateMap<BeautyServiceRequest, BeautyService>().ReverseMap();
            CreateMap<BeautyService, BeautyServiceResponse>().ReverseMap();
        }
    }
}
