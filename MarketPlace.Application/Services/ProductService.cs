using System.Linq.Expressions;
using AutoMapper;
using MarketPlace.Application.Exceptions;
using MarketPlace.Application.Helpers.Expressions;
using MarketPlace.Application.Interfaces.Persistent;
using MarketPlace.Application.Interfaces.Services;
using MarketPlace.Application.Models.Requests.Products;
using MarketPlace.Application.Models.Results.Products;
using MarketPlace.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MarketPlace.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository productRepository, IMapper mapper, ILogger<ProductService> logger, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductResult> GetByIdAsync(string id)
    {
        var result = await _productRepository.GetByIdAsync<ProductResult>(id);

        if (result == null)
        {
            throw new NotFoundException($"Product with id \"{id}\" not found");
        }

        return result;
    }

    public async Task<List<ProductResult>> GetAsync(GetProductsRequest request)
    {
        var predicate = CreateFilterPredicate(request);
        var result = await _productRepository.GetAsync<ProductResult>(predicate);
        return result;
    }

    public async Task<ProductResult> CreateAsync(string ownerShopId, CreateProductRequest request)
    {
        var productExists = await _productRepository.ExistsAsync(x =>
            x.Name == request.Name && x.ShopId == ownerShopId);

        if (productExists)
            throw new ValidationException("Product with such parameters already exists");
        
        var productEntity = _mapper.Map<CreateProductRequest, Product>(request);
        productEntity.ShopId = ownerShopId;
        productEntity.CreatedBy = ownerShopId;

        if (request.CategoriesIds?.Any() == true)
        {
            var categories = await _categoryRepository.GetAsync(x => request.CategoriesIds.Contains(x.Id));
            productEntity.ProductCategories = categories.Select(x => new ProductCategory()
            {
                Category = x,
                Product = productEntity
            }).ToList();
        }
        
        await _productRepository.AddAsync(productEntity);
        _logger.LogInformation("New Product {@Entity} was created successfully", productEntity);
        var result = _mapper.Map<Product, ProductResult>(productEntity); 
        return result;
    }

    public async Task<ProductResult> UpdateAsync(string id, UpdateProductRequest request)
    {
        var productToUpdate = await _productRepository.GetByIdAsync(id);

        if (productToUpdate == null)
            throw new NotFoundException($"Product with such id {id} not found");

        _mapper.Map(request, productToUpdate);
        await _productRepository.UpdateAsync(productToUpdate);
        _logger.LogInformation("Product {@Entity} was updated successfully", productToUpdate);
        var result = _mapper.Map<Product, ProductResult>(productToUpdate); 
        return result;
    }

    public async Task DeleteAsync(string id)
    {
        var productToDelete = await _productRepository.GetByIdAsync(id);

        if (productToDelete == null)
            throw new NotFoundException($"Product with such id {id} not found");

        await _productRepository.DeleteAsync(productToDelete);
        _logger.LogInformation("Product with id {ProductId} was deleted successfully", productToDelete.Id);
    }
    
    private Expression<Func<Product, bool>>? CreateFilterPredicate(GetProductsRequest request)
    {
        Expression<Func<Product, bool>>? predicate = null;

        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            Expression<Func<Product, bool>> searchStringExpression = x => x.Name.Contains(request.SearchString);
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