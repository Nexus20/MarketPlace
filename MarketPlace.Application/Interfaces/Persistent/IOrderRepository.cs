using MarketPlace.Domain.Entities;

namespace MarketPlace.Application.Interfaces.Persistent;

public interface IOrderRepository : IRepository<Order>
{
    Task AddAsync(Order orderToAdd, List<Product> productsToBuy);
}