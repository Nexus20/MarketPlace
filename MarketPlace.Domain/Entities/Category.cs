using MarketPlace.Domain.Entities.Abstract;

namespace MarketPlace.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;
    
    public virtual List<ProductCategory>? ProductCategories { get; set; }
}