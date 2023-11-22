using AutoMapper;
using ShopMate.Core.Entities;

namespace ShopMate.WebApi.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Favourite, ProductFavourite>();
        }
    }
}
