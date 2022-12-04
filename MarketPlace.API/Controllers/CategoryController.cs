using MarketPlace.Application.Interfaces.Services;
using MarketPlace.Application.Models.Requests.Categories;
using MarketPlace.Application.Models.Results.Categories;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase {
        
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CategoryResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery]GetCategoriesRequest request)
    {
        var result = await _categoryService.GetAsync(request);
        return Ok(result);
    }
        
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _categoryService.GetByIdAsync(id);
        return Ok(result);
    }
        
    [HttpPost]
    [ProducesResponseType(typeof(CategoryResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody]CreateCategoryRequest request)
    {
        var result = await _categoryService.CreateAsync(request);
        return StatusCode(StatusCodes.Status201Created, result);
    }
        
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CategoryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(string id, [FromBody]UpdateCategoryRequest request)
    {
        var result = await _categoryService.UpdateAsync(id, request);
        return Ok(result);
    }
        
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _categoryService.DeleteAsync(id);
        return NoContent();
    }
}