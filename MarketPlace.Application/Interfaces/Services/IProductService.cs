using MarketPlace.Application.Models.Requests.Products;
using MarketPlace.Application.Models.Results.Products;

namespace MarketPlace.Application.Interfaces.Services;

public interface IProductService
{
    public Task<ProductResult> GetByIdAsync(string id);
    public Task<List<ProductResult>> GetAsync(GetProductsRequest request);
    public Task<ProductResult> CreateAsync(string ownerShopId, CreateProductRequest request);
    public Task<ProductResult> UpdateAsync(string id, UpdateProductRequest request, string ownerShopId);
    public Task DeleteAsync(string id);
}