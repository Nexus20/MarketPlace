using MarketPlace.Domain.Entities.Abstract;

namespace MarketPlace.Domain.Entities;

public class Shop : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? SiteUrl { get; set; }
    public virtual List<Product>? Products { get; set; }
    public string UserId { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual List<Order>? Orders { get; set; }
}