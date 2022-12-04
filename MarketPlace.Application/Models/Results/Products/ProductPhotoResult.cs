using MarketPlace.Application.Models.Results.Abstract;

namespace MarketPlace.Application.Models.Results.Products;

public class ProductPhotoResult : BaseResult
{
    public string Path { get; set; } = null!;
    public string ProductId { get; set; } = null!;
}