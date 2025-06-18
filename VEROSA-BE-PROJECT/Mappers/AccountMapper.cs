using AutoMapper;
using VEROSA.Common.Enums;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;
using VEROSA.DataAccessLayer.Entities;

namespace VEROSA_BE_PROJECT.Mappers
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<RegisterRequest, Account>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => AccountRole.Customer))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AccountStatus.Active))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
