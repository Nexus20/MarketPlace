using MarketPlace.Application.Models.Requests.Orders;
using MarketPlace.Application.Models.Results.Orders;
using MarketPlace.Domain.Enums;

namespace MarketPlace.Application.Interfaces.Services;

public interface IOrderService
{
    Task<OrderResult> CreateAsync(string buyerId, CreateOrderRequest request);
    Task<List<OrderResult>> GetWithDetailsAsync(GetOrdersRequest request);
    Task<List<ShopOrderResult>> GetShopOrdersAsync(GetOrdersRequest request);
    Task<List<BuyerOrderResult>> GetBuyerOrdersAsync(GetOrdersRequest request);
    Task<OrderResult> GetByIdAsync(string id);
    Task<OrderStatus> ChangeStatusAsync(string id, ChangeOrderStatusRequest request);
}