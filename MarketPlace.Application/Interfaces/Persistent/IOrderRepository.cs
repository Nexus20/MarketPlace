using System.Linq.Expressions;
using MarketPlace.Domain.Entities;

namespace MarketPlace.Application.Interfaces.Persistent;

public interface IOrderRepository : IRepository<Order>
{
    Task AddAsync(Order orderToAdd, List<Product> productsToBuy);

    Task<List<Order>> GetWithDetailsAsync(Expression<Func<Order, bool>>? predicate = null);
    Task<Order?> GetByIdWithDetailsAsync(string id);
}