using AutoMapper;
using MarketPlace.Application.Models.Requests.Shops;
using MarketPlace.Application.Models.Results.Shops;
using MarketPlace.Domain.Entities;

namespace MarketPlace.Application.Mappings;

public class ShopProfile : Profile
{
    public ShopProfile()
    {
        CreateMap<CreateShopRequest, Shop>()
            .ForMember(x => x.User, o => o.MapFrom(s => s));
        CreateMap<CreateShopRequest, User>();

        CreateMap<Shop, ShopResult>()
            .ForMember(x => x.Email, o => o.MapFrom(s => s.User.Email))
            .ForMember(x => x.Phone, o => o.MapFrom(s => s.User.Phone));
    }
}