using MarketPlace.Domain.Entities.Abstract;

namespace MarketPlace.Domain.Entities;

public class ProductCategory : BaseEntity
{
    public string CategoryId { get; set; } = null!;
    public virtual Category Category { get; set; } = null!;
    public string ProductId { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}