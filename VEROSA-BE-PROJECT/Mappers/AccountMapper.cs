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
            CreateMap<Account, AccountResponse>();
            CreateMap<RegisterRequest, Account>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(_ => AccountRole.Customer))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => AccountStatus.Active));
        }
    }
}
