using MarketPlace.Application.Models.Results.Abstract;

namespace MarketPlace.Application.Models.Results.Buyers;

public class BuyerResult : BaseResult
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
}