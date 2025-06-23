using AutoMapper;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;
using VEROSA.DataAccessLayer.Entities;

namespace VEROSA_BE_PROJECT.Mappers
{
    public class SupportTicketMapper : Profile
    {
        public SupportTicketMapper()
        {
            CreateMap<SupportTicketRequest, SupportTicket>().ReverseMap();
            CreateMap<SupportTicket, SupportTicketResponse>()
                .ForMember(
                    dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer.Username)
                )
                .ReverseMap();
        }
    }
}
