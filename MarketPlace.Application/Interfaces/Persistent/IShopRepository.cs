using MarketPlace.Domain.Entities;

namespace MarketPlace.Application.Interfaces.Persistent;

public interface IShopRepository : IRepository<Shop>
{
    Task AddAsync(Shop shopEntity, string password);
}