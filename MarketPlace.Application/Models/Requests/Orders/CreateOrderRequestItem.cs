using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Application.Models.Requests.Orders;

public class CreateOrderRequestItem
{
    [Required] public int Count { get; set; }
    [Required] public string ProductId { get; set; } = null!;
}