using MarketPlace.Application.Interfaces.Services;
using MarketPlace.Application.Models.Requests.Auth;
using MarketPlace.Application.Models.Results.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.API.Controllers
{
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISignInService _signInService;

        public UsersController(ISignInService signInService)
        {
            _signInService = signInService;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(LoginResult), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _signInService.SignInAsync(request);
            return result.IsAuthSuccessful ? Ok(result) : Unauthorized(result);
        }
    }
}
