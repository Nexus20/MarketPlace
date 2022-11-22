using MarketPlace.Application.Interfaces.Services;
using MarketPlace.Application.Models.Requests.Categories;
using MarketPlace.Application.Models.Requests.Shops;
using MarketPlace.Application.Models.Results.Categories;
using MarketPlace.Application.Models.Results.Shops;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.API.Controllers {
    
    [ApiController]
    [Route("[controller]")]
    public class ShopController : ControllerBase {
        
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ShopResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery]GetShopsRequest request)
        {
            var result = await _shopService.GetAsync(request);
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
        [ProducesResponseType(typeof(ShopResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody]CreateShopRequest request)
        {
            var result = await _shopService.CreateAsync(request);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ShopResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(string id, [FromBody]UpdateShopRequest request)
        {
            var result = await _shopService.UpdateAsync(id, request);
            return Ok(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _shopService.DeleteAsync(id);
            return NoContent();
        }
    }
}