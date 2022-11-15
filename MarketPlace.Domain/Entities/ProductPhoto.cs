using MarketPlace.Domain.Entities.Abstract;

namespace MarketPlace.Domain.Entities;

public class ProductPhoto : BaseEntity
{
    public string Path { get; set; } = null!;
    
    public string ProductId { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}