using MarketPlace.Application.Models.Results.Abstract;
using MarketPlace.Application.Models.Results.Categories;

namespace MarketPlace.Application.Models.Results.Products;

public class ProductResult : BaseResult
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal? Discount { get; set; }
    public int Count { get; set; }
    public virtual List<CategoryResult>? Categories { get; set; }
}