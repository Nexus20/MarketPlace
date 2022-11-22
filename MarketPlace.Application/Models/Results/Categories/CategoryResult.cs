using MarketPlace.Application.Models.Results.Abstract;
using MarketPlace.Application.Models.Results.Products;

namespace MarketPlace.Application.Models.Results.Categories;

public class CategoryResult : BaseResult
{
    public string Name { get; set; } = null!;
    
    public List<ProductResult>? Products { get; set; }
}