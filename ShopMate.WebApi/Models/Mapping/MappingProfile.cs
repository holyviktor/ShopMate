using AutoMapper;
using ShopMate.Core.Entities;
using ShopMate.Core.Models;

namespace ShopMate.WebApi.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Favourite, ProductFavourite>();
            this.CreateMap<Review, ProductReview>();
            this.CreateMap<User, UserForReview>();
            this.CreateMap<Basket, ProductBasket>();
            this.CreateMap<User, UserProfile>();
            this.CreateMap<User, UserOrder>();
            this.CreateMap<OrderInput, OrderCreationInfo>();
            this.CreateMap<CreateOrder, OrderCreationInfo>();
            this.CreateMap<Address, UserAddressModel>();
            this.CreateMap<Coupon, CouponModel>();
            this.CreateMap<OrderProduct, ProductBasket>();
        }
    }
}
