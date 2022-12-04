using AutoMapper;
using MarketPlace.Application.Models.Requests.Auth;
using MarketPlace.Application.Models.Results.Buyers;
using MarketPlace.Domain.Entities;

namespace MarketPlace.Application.Mappings;

public class BuyerProfile : Profile
{
    public BuyerProfile()
    {
        CreateMap<RegisterBuyerRequest, Buyer>()
            .ForMember(x => x.User, o => o.MapFrom(s => s));
        CreateMap<RegisterBuyerRequest, User>();

        CreateMap<Buyer, BuyerResult>()
            .ForMember(x => x.Email, o => o.MapFrom(s => s.User.Email))
            .ForMember(x => x.Phone, o => o.MapFrom(s => s.User.Phone));
    }
}