using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Application.Models.Requests.Shops;

public class UpdateShopRequest
{
    [Required] public string Name { get; set; } = null!;
}