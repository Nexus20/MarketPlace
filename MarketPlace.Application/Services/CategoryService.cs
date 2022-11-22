using System.Linq.Expressions;
using AutoMapper;
using MarketPlace.Application.Exceptions;
using MarketPlace.Application.Helpers.Expressions;
using MarketPlace.Application.Interfaces.Persistent;
using MarketPlace.Application.Interfaces.Services;
using MarketPlace.Application.Models.Requests.Categories;
using MarketPlace.Application.Models.Results.Categories;
using MarketPlace.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MarketPlace.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, ILogger<CategoryService> logger)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CategoryResult> GetByIdAsync(string id)
    {
        var result = await _categoryRepository.GetByIdAsync<CategoryResult>(id);

        if (result == null)
        {
            throw new NotFoundException($"category with id \"{id}\" not found");
        }

        return result;
    }

    public async Task<List<CategoryResult>> GetAsync(GetCategoriesRequest request)
    {
        var predicate = CreateFilterPredicate(request);
        var result = await _categoryRepository.GetAsync(predicate);
        return _mapper.Map<List<Category>, List<CategoryResult>>(result);
    }

    public async Task<CategoryResult> CreateAsync(CreateCategoryRequest request)
    {
        var categoryExists = await _categoryRepository.ExistsAsync(x => x.Name == request.Name);

        if (categoryExists)
            throw new ValidationException("category with such parameters already exists");
        
        var categoryEntity = _mapper.Map<CreateCategoryRequest, Category>(request);
        await _categoryRepository.AddAsync(categoryEntity);
        _logger.LogInformation("New category {@Entity} was created successfully", categoryEntity);
        var result = _mapper.Map<Category, CategoryResult>(categoryEntity); 
        return result;
    }

    public async Task<CategoryResult> UpdateAsync(string id, UpdateCategoryRequest request)
    {
        var categoryToUpdate = await _categoryRepository.GetByIdAsync(id);

        if (categoryToUpdate == null)
            throw new NotFoundException($"Category with such id {id} not found");

        _mapper.Map(request, categoryToUpdate);
        await _categoryRepository.UpdateAsync(categoryToUpdate);
        _logger.LogInformation("Category {@Entity} was updated successfully", categoryToUpdate);
        var result = _mapper.Map<Category, CategoryResult>(categoryToUpdate); 
        return result;
    }

    public async Task DeleteAsync(string id)
    {
        var categoryToDelete = await _categoryRepository.GetByIdAsync(id);

        if (categoryToDelete == null)
            throw new NotFoundException($"Category with such id {id} not found");

        await _categoryRepository.DeleteAsync(categoryToDelete);
        _logger.LogInformation("Category with id {CategoryId} was deleted successfully", categoryToDelete.Id);
    }
    
    private Expression<Func<Category, bool>>? CreateFilterPredicate(GetCategoriesRequest request)
    {
        Expression<Func<Category, bool>>? predicate = null;

        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            Expression<Func<Category, bool>> searchStringExpression = x => x.Name.Contains(request.SearchString);
            predicate = ExpressionsHelper.And(predicate, searchStringExpression);
        }
        //
        // if (request.Status.HasValue && Enum.IsDefined(request.Status.Value))
        // {
        //     Expression<Func<HelpRequest, bool>> statusPredicate = x => x.Status == request.Status.Value;
        //     predicate = ExpressionsHelper.And(predicate, statusPredicate);
        // }
        //
        // if (request.StartDate.HasValue && request.EndDate.HasValue && request.StartDate < request.EndDate)
        // {
        //     Expression<Func<HelpRequest, bool>> dateExpression = x => x.CreatedDate > request.StartDate.Value
        //                                                               && x.CreatedDate < request.EndDate.Value;
        //     predicate = ExpressionsHelper.And(predicate, dateExpression);
        // }

        return predicate;
    }
}