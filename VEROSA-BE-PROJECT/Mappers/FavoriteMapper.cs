using AutoMapper;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;
using VEROSA.DataAccessLayer.Entities;

namespace VEROSA_BE_PROJECT.Mappers
{
    public class FavoriteMapper : Profile
    {
        public FavoriteMapper()
        {
            CreateMap<FavoriteRequest, Favorite>().ReverseMap();
            CreateMap<Favorite, FavoriteResponse>().ReverseMap();
        }
    }
}
