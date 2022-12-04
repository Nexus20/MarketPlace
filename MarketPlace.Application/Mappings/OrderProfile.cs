using AutoMapper;
using MarketPlace.Application.Models.Requests.Orders;
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
        
        CreateMap<Order, ShopOrderResult>()
            .ForMember(x => x.Items, o => o.MapFrom(s => s.Items));
        
        CreateMap<Order, BuyerOrderResult>()
            .ForMember(x => x.Items, o => o.MapFrom(s => s.Items));

        CreateMap<OrderItem, OrderItemResult>()
            .ForMember(x => x.ProductName, o => o.MapFrom(s => s.Product.Name))
            .ForMember(x => x.Cost, o => o.MapFrom(s => s.Count * s.Product.FinalPrice));
    }
}