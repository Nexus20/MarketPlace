using AutoMapper;
using MarketPlace.Application.Models.Requests.Auth;
using MarketPlace.Application.Models.Requests.Orders;
using MarketPlace.Application.Models.Results.Buyers;
using MarketPlace.Application.Models.Results.Orders;
using MarketPlace.Domain.Entities;

namespace MarketPlace.Application.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<CreateOrderRequest, Order>();

        CreateMap<Order, OrderResult>()
            .ForMember(x => x.Items, o => o.MapFrom(s => s.Items));

        CreateMap<OrderItem, OrderItemResult>()
            .ForMember(x => x.ProductName, o => o.MapFrom(s => s.Product.Name))
            .ForMember(x => x.Cost, o => o.MapFrom(s => s.Count * s.Product.FinalPrice));
    }
}

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