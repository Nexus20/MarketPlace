using MarketPlace.Domain.Entities.Abstract;

namespace MarketPlace.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public virtual List<ProductPhoto>? Photos { get; set; }
    public decimal Price { get; set; }
    public decimal? Discount { get; set; }
    public int Count { get; set; }
    
    public string ShopId { get; set; } = null!;
    public virtual Shop Shop { get; set; } = null!;
    
    public virtual List<OrderItem>? Items { get; set; }
    
    public virtual List<ProductCategory> ProductCategories { get; set; } = null!;
    public decimal FinalPrice => Discount.HasValue ? Price - Price * (Discount.Value / 100) : Price;
}