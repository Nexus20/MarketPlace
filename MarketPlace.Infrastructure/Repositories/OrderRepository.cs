using System.Linq.Expressions;
using AutoMapper;
using MarketPlace.Application.Interfaces.Persistent;
using MarketPlace.Domain.Entities;
using MarketPlace.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
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

    public Task<List<Order>> GetWithDetailsAsync(Expression<Func<Order, bool>>? predicate = null)
    {
        var query = DbContext.Orders
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .ThenInclude(x => x.ProductCategories)
            .ThenInclude(x => x.Category)
            .Include(x => x.Shop)
            .ThenInclude(x => x.User)
            .Include(x => x.Buyer)
            .ThenInclude(x => x.User)
            .AsNoTracking();

        if (predicate != null)
            query = query.Where(predicate);

        return query.ToListAsync();
    }

    public Task<Order?> GetByIdWithDetailsAsync(string id)
    {
        return DbContext.Orders
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .ThenInclude(x => x.ProductCategories)
            .ThenInclude(x => x.Category)
            .Include(x => x.Shop)
            .ThenInclude(x => x.User)
            .Include(x => x.Buyer)
            .ThenInclude(x => x.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}