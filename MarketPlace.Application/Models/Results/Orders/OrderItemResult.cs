using MarketPlace.Application.Models.Results.Abstract;

namespace MarketPlace.Application.Models.Results.Orders;

public class OrderItemResult : BaseResult
{
    public int Count { get; set; }
    public string ProductId { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public decimal Cost { get; set; }
}