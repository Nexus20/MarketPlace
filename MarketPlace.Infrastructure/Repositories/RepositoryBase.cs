using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MarketPlace.Application.Interfaces.Persistent;
using MarketPlace.Application.Models.Results.Abstract;
using MarketPlace.Domain.Entities.Abstract;
using MarketPlace.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Infrastructure.Repositories;

public class RepositoryBase<TEntity> : IAsyncRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly IMapper Mapper;
    protected readonly ApplicationDbContext DbContext;

    public RepositoryBase(ApplicationDbContext dbContext, IMapper mapper)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        Mapper = mapper;
    }

    public Task<List<TEntity>> GetAllAsync()
    {
        return DbContext.Set<TEntity>().ToListAsync();
    }

    public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (predicate == null)
            return GetAllAsync();

        return DbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string? includeString = null,
        bool disableTracking = true)
    {
        IQueryable<TEntity> query = DbContext.Set<TEntity>();
        if (disableTracking) query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null)
            return orderBy(query).ToListAsync();
        return query.ToListAsync();
    }

    public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        List<Expression<Func<TEntity, BaseEntity>>>? includes = null,
        bool disableTracking = true)
    {
        IQueryable<TEntity> query = DbContext.Set<TEntity>();
        if (disableTracking) query = query.AsNoTracking();

        if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null)
            return orderBy(query).ToListAsync();
        return query.ToListAsync();
    }

    public virtual Task<List<TResult>> GetAsync<TResult>(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        List<Expression<Func<TEntity, BaseEntity>>>? includes = null,
        bool disableTracking = true) where TResult : BaseResult
    {
        IQueryable<TEntity> query = DbContext.Set<TEntity>();
        if (disableTracking) query = query.AsNoTracking();

        if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null)
            query = orderBy(query);

        return query.ProjectTo<TResult>(Mapper.ConfigurationProvider).ToListAsync();
    }

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return DbContext.Set<TEntity>().AnyAsync(predicate);
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate)
    {
        if (predicate == null)
            return DbContext.Set<TEntity>().CountAsync();

        return DbContext.Set<TEntity>().CountAsync(predicate);
    }

    public virtual Task<TEntity?> GetByIdAsync(string id)
    {
        return DbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public virtual Task<TResult?> GetByIdAsync<TResult>(string id) where TResult : BaseResult
    {
        return DbContext.Set<TEntity>()
            .ProjectTo<TResult>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);
        await DbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        DbContext.Entry(entity).State = EntityState.Modified;
        await DbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
        await DbContext.SaveChangesAsync();
    }
}