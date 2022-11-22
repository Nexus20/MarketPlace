using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Application.Models.Requests.Products;

public class UpdateProductRequest
{
    [Required] public string Name { get; set; } = null!;
}