using System.Security.Claims;
using MarketPlace.Application.Authorization;
using MarketPlace.Application.Interfaces.Services;
using MarketPlace.Application.Models.Requests.Orders;
using MarketPlace.Application.Models.Results.Orders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.API.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("[controller]")]
public class BuyerController : ControllerBase
{
    private readonly IOrderService _orderService;

    public BuyerController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("[action]")]
    [Authorize(Roles = CustomRoles.Buyer)]
    [ProducesResponseType(typeof(List<OrderResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOwnOrders([FromQuery] GetOrdersRequest request)
    {
        var ownerShopId = User.FindFirstValue(CustomClaimTypes.BuyerId);

        if (string.IsNullOrWhiteSpace(ownerShopId))
            return Forbid();

        request.BuyerId = ownerShopId;

        var result = await _orderService.GetWithDetailsAsync(request);
        return Ok(result);
    }
}