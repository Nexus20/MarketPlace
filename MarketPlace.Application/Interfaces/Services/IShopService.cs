using MarketPlace.Application.Models.Requests.Shops;
using MarketPlace.Application.Models.Results.Shops;

namespace MarketPlace.Application.Interfaces.Services;

public interface IShopService
{
    public Task<ShopResult> GetByIdAsync(string id);
    public Task<List<ShopResult>> GetAsync(GetShopsRequest request);
    public Task<ShopResult> CreateAsync(CreateShopRequest request);
    public Task<ShopResult> UpdateAsync(string id, UpdateShopRequest request);
    public Task DeleteAsync(string id);
}