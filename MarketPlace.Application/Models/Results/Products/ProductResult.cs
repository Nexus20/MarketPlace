using MarketPlace.Application.Models.Results.Abstract;
using MarketPlace.Application.Models.Results.Categories;
using MarketPlace.Domain.Entities;

namespace MarketPlace.Application.Models.Results.Products;

public class ProductResult : BaseResult
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal? Discount { get; set; }
    public int Count { get; set; }
    public string ShopId { get; set; } = null!;
    public List<CategoryResult>? Categories { get; set; }
    public List<ProductPhotoResult>? Photos { get; set; }
}