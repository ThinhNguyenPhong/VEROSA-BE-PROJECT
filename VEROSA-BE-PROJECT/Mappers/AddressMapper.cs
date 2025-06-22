using AutoMapper;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;
using VEROSA.DataAccessLayer.Entities;

namespace VEROSA_BE_PROJECT.Mappers
{
    public class AddressMapper : Profile
    {
        public AddressMapper()
        {
            CreateMap<AddressRequest, Address>().ReverseMap();
            CreateMap<Address, AddressResponse>().ReverseMap();
        }
    }
}
