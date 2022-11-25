using System.Security.Claims;
using MarketPlace.Application.Authorization;
using MarketPlace.Application.Interfaces.Services;
using MarketPlace.Application.Models.Requests.Orders;
using MarketPlace.Application.Models.Requests.Products;
using MarketPlace.Application.Models.Requests.Shops;
using MarketPlace.Application.Models.Results.Products;
using MarketPlace.Application.Models.Results.Shops;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.API.Controllers {
    
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    public class OrderController : ControllerBase {
        
        private readonly IShopService _shopService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public OrderController(IShopService shopService, IProductService productService, IOrderService orderService)
        {
            _shopService = shopService;
            _productService = productService;
            _orderService = orderService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ShopResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery]GetShopsRequest request)
        {
            var result = await _shopService.GetAsync(request);
            return Ok(result);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = CustomRoles.Shop)]
        [ProducesResponseType(typeof(List<ProductResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOwnProducts([FromQuery]GetProductsRequest request)
        {
            var ownerShopId = User.FindFirstValue(CustomClaimTypes.ShopId);

            if (string.IsNullOrWhiteSpace(ownerShopId))
                return Forbid();

            request.ShopId = ownerShopId;
            
            var result = await _productService.GetAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ShopResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _shopService.GetByIdAsync(id);
            return Ok(result);
        }
        
        [HttpPost]
        [Authorize(Roles = CustomRoles.Buyer)]
        [ProducesResponseType(typeof(ShopResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody]CreateOrderRequest request)
        {
            var buyerId = User.FindFirstValue(CustomClaimTypes.BuyerId);
            var result = await _orderService.CreateAsync(buyerId, request);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _shopService.DeleteAsync(id);
            return NoContent();
        }
    }
}