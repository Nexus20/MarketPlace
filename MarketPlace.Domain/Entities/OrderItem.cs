using MarketPlace.Domain.Entities.Abstract;

namespace MarketPlace.Domain.Entities;

public class OrderItem : BaseEntity
{
    public string ProductId { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
    public string OrderId { get; set; } = null!;
    public virtual Order Order { get; set; } = null!;
    public int Count { get; set; }
}