using MarketPlace.Application.Models.Requests.Categories;
using MarketPlace.Application.Models.Results.Categories;

namespace MarketPlace.Application.Interfaces.Services;

public interface ICategoryService
{
    public Task<CategoryResult> GetByIdAsync(string id);
    public Task<List<CategoryResult>> GetAsync(GetCategoriesRequest request);
    public Task<CategoryResult> CreateAsync(CreateCategoryRequest request);
    public Task<CategoryResult> UpdateAsync(string id, UpdateCategoryRequest request);
    public Task DeleteAsync(string id);
}