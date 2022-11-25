using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Application.Models.Requests.Orders;

public class CreateOrderRequest
{
    [Required]
    public string ShopId { get; set; } = null!;
    [Required]
    public string Country { get; set; } = null!;
    [Required]
    public string City { get; set; } = null!;
    [Required]
    public string Address { get; set; } = null!;
    [Required]
    public List<CreateOrderRequestItem> OrderItems { get; set; } = null!;
}