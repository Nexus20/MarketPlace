using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Application.Models.Requests.Shops;

public class CreateShopRequest
{
    [Required] public string Name { get; set; } = null!;
    [Required] public string Phone { get; set; } = null!;
    [Required] public string Email { get; set; } = null!;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? SiteUrl { get; set; }
    [Required] public string Password { get; set; } = null!; // _QGrXyvcmTD4aVQJ_ for tests
}