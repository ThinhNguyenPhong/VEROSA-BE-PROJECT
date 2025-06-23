using AutoMapper;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;
using VEROSA.DataAccessLayer.Entities;

namespace VEROSA_BE_PROJECT.Mappers
{
    public class BlogPostMapper : Profile
    {
        public BlogPostMapper()
        {
            CreateMap<BlogPostRequest, BlogPost>().ReverseMap();
            CreateMap<BlogPost, BlogPostResponse>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Username))
                .ReverseMap();
        }
    }
}
