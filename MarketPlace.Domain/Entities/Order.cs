using MarketPlace.Domain.Entities.Abstract;
using MarketPlace.Domain.Enums;

namespace MarketPlace.Domain.Entities;

public class Order : BaseEntity
{
    public virtual string BuyerId { get; set; } = null!;
    public virtual Buyer Buyer { get; set; } = null!;
    
    public virtual string ShopId { get; set; } = null!;
    public virtual Shop Shop { get; set; } = null!;
    
    public virtual List<OrderItem> Items { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string DeliveryAddress { get; set; } = null!;
}