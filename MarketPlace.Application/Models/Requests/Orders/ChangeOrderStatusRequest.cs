using System.ComponentModel.DataAnnotations;
using MarketPlace.Application.Validation.Attributes;
using MarketPlace.Domain.Enums;

namespace MarketPlace.Application.Models.Requests.Orders;

public class ChangeOrderStatusRequest
{
    [Required]
    [ValidateEnum]
    public OrderStatus NewStatus { get; set; }
}