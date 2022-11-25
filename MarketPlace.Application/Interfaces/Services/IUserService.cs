using MarketPlace.Application.Models.Requests.Auth;
using MarketPlace.Application.Models.Results.Buyers;

namespace MarketPlace.Application.Interfaces.Services;

public interface IUserService
{
    Task<BuyerResult> RegisterBuyerAsync(RegisterBuyerRequest request);
}