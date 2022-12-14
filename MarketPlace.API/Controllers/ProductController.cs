using System.Security.Claims;
using MarketPlace.Application.Authorization;
using MarketPlace.Application.Interfaces.Services;
using MarketPlace.Application.Models.Dtos;
using MarketPlace.Application.Models.Requests.Products;
using MarketPlace.Application.Models.Results.Products;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.API.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("[controller]")]
public class ProductController : ControllerBase {
        
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<ProductResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery]GetProductsRequest request)
    {
        var result = await _productService.GetAsync(request);
        return Ok(result);
    }
        
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ProductResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _productService.GetByIdAsync(id);
        return Ok(result);
    }
        
    [HttpPost]
    [Authorize(Roles = CustomRoles.Shop)]
    [ProducesResponseType(typeof(ProductResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody]CreateProductRequest request)
    {
        var ownerShopId = User.FindFirstValue(CustomClaimTypes.ShopId);

        if (string.IsNullOrWhiteSpace(ownerShopId))
            return Forbid();
            
        var result = await _productService.CreateAsync(ownerShopId, request);
        return StatusCode(StatusCodes.Status201Created, result);
    }
        
    [HttpPut("{id}")]
    [Authorize(Roles = CustomRoles.Shop)]
    [ProducesResponseType(typeof(ProductResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(string id, [FromBody]UpdateProductRequest request)
    {
        var ownerShopId = User.FindFirstValue(CustomClaimTypes.ShopId);

        if (string.IsNullOrWhiteSpace(ownerShopId))
            return Forbid();
            
        var result = await _productService.UpdateAsync(id, request, ownerShopId);
        return Ok(result);
    }
        
    [HttpPatch("{id}/[action]")]
    [Authorize(Roles = CustomRoles.Shop)]
    [ProducesResponseType(typeof(List<ProductPhotoResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddPhotos(string id)
    {
        var ownerShopId = User.FindFirstValue(CustomClaimTypes.ShopId);

        if (string.IsNullOrWhiteSpace(ownerShopId))
            return Forbid();

        if (!Request.Form.Files.Any())
            return BadRequest();
            
        var filesDtos = new List<FileDto>();

        foreach (var formFile in Request.Form.Files)
        {
            filesDtos.Add(new FileDto()
            {
                Content = formFile.OpenReadStream(),
                Name = formFile.FileName,
                ContentType = formFile.ContentType
            });
        }
            
        var result = await _productService.AddImagesToProductAsync(id, filesDtos, ownerShopId);
        return Ok(result);
    }
        
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _productService.DeleteAsync(id);
        return NoContent();
    }
}