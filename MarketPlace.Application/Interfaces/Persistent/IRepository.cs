using System.Linq.Expressions;
using MarketPlace.Application.Models.Results.Abstract;
using MarketPlace.Domain.Entities.Abstract;

namespace MarketPlace.Application.Interfaces.Persistent;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate);

    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeString = null,
        bool disableTracking = true);

    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        List<Expression<Func<TEntity, BaseEntity>>>? includes = null,
        bool disableTracking = true);
    
    Task<List<TResult>> GetAsync<TResult>(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        List<Expression<Func<TEntity, BaseEntity>>>? includes = null,
        bool disableTracking = true) where TResult : BaseResult;

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate);
    Task<TEntity?> GetByIdAsync(string id);
    Task<TResult?> GetByIdAsync<TResult>(string id) where TResult : BaseResult;
    Task<TEntity> AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}