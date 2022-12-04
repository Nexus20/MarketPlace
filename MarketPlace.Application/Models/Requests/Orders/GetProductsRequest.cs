namespace MarketPlace.Application.Models.Requests.Orders;

public class GetOrdersRequest
{
    public string? ShopId { get; set; }
    public string? BuyerId { get; set; }
}