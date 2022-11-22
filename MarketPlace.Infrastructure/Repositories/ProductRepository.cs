using AutoMapper;
using MarketPlace.Application.Interfaces.Persistent;
using MarketPlace.Domain.Entities;
using MarketPlace.Infrastructure.Persistence;

namespace MarketPlace.Infrastructure.Repositories;

public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}