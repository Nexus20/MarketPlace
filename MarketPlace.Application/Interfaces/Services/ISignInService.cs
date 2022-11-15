using MarketPlace.Application.Models.Requests.Auth;
using MarketPlace.Application.Models.Results.Auth;

namespace MarketPlace.Application.Interfaces.Services;

public interface ISignInService {

    Task<LoginResult> SignInAsync(LoginRequest request);
}