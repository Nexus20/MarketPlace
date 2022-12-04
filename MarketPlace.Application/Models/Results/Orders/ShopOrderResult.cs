using MarketPlace.Application.Models.Results.Abstract;
using MarketPlace.Application.Models.Results.Buyers;
using MarketPlace.Domain.Enums;

namespace MarketPlace.Application.Models.Results.Orders;

public class ShopOrderResult : BaseResult
{
    public BuyerResult Buyer { get; set; } = null!;
    public List<OrderItemResult> Items { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string Country { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Address { get; set; } = null!;
    public decimal Cost { get; set; }
}