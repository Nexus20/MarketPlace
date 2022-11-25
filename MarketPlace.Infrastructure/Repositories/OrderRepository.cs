using AutoMapper;
using MarketPlace.Application.Interfaces.Persistent;
using MarketPlace.Domain.Entities;
using MarketPlace.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace MarketPlace.Infrastructure.Repositories;

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(ApplicationDbContext dbContext, IMapper mapper, ILogger<OrderRepository> logger) : base(dbContext, mapper)
    {
        _logger = logger;
    }

    public async Task AddAsync(Order orderToAdd, List<Product> productsToBuy)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync();
        try
        {
            await DbContext.Orders.AddAsync(orderToAdd);
            DbContext.Products.UpdateRange(productsToBuy);

            await DbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            _logger.LogError("Error while creating a new order: {EMessage}", e.Message);
        }
    }
}