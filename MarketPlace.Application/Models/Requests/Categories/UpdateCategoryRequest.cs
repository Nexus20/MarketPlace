using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Application.Models.Requests.Categories;

public class UpdateCategoryRequest
{
    [Required] public string Name { get; set; } = null!;
}