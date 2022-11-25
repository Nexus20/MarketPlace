using MarketPlace.Application.Models.Requests.Orders;
using MarketPlace.Application.Models.Results.Orders;

namespace MarketPlace.Application.Interfaces.Services;

public interface IOrderService
{
    Task<OrderResult> CreateAsync(string buyerId, CreateOrderRequest request);
}