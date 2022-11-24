using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Application.Models.Requests.Products;

public class UpdateProductRequest
{
    [Required]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    [Required]
    public decimal Price { get; set; }
    public decimal? Discount { get; set; }
    public int Count { get; set; } = 0;
    public List<string>? CategoriesIds { get; set; }
}