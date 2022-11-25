using MarketPlace.Domain.Entities;

namespace MarketPlace.Application.Interfaces.Persistent;

public interface IBuyerRepository : IRepository<Buyer>
{
    Task AddAsync(Buyer buyerEntity, string password);
}