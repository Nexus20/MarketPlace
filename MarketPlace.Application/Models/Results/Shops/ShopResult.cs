using MarketPlace.Application.Models.Results.Abstract;

namespace MarketPlace.Application.Models.Results.Shops;

public class ShopResult : BaseResult
{
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? SiteUrl { get; set; }
}