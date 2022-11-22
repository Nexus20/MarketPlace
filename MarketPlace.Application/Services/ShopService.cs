using System.Linq.Expressions;
using AutoMapper;
using MarketPlace.Application.Exceptions;
using MarketPlace.Application.Helpers.Expressions;
using MarketPlace.Application.Interfaces.Persistent;
using MarketPlace.Application.Interfaces.Services;
using MarketPlace.Application.Models.Requests.Categories;
using MarketPlace.Application.Models.Requests.Shops;
using MarketPlace.Application.Models.Results.Categories;
using MarketPlace.Application.Models.Results.Shops;
using MarketPlace.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MarketPlace.Application.Services;

public class ShopService : IShopService
{
    private readonly IShopRepository _shopRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ShopService> _logger;

    public ShopService(IShopRepository shopRepository, IMapper mapper, ILogger<ShopService> logger)
    {
        _shopRepository = shopRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ShopResult> GetByIdAsync(string id)
    {
        var result = await _shopRepository.GetByIdAsync<ShopResult>(id);

        if (result == null)
        {
            throw new NotFoundException($"Shop with id \"{id}\" not found");
        }

        return result;
    }

    public async Task<List<ShopResult>> GetAsync(GetShopsRequest request)
    {
        var predicate = CreateFilterPredicate(request);
        var result = await _shopRepository.GetAsync<ShopResult>(predicate);
        return result;
    }

    public async Task<ShopResult> CreateAsync(CreateShopRequest request)
    {
        var shopExists = await _shopRepository.ExistsAsync(x =>
            x.Name == request.Name && x.User.Email == request.Email && x.User.Phone == request.Phone);

        if (shopExists)
            throw new ValidationException("Shop with such parameters already exists");
        
        var shopEntity = _mapper.Map<CreateShopRequest, Shop>(request);
        await _shopRepository.AddAsync(shopEntity, request.Password);
        _logger.LogInformation("New Shop {@Entity} was created successfully", shopEntity);
        var result = _mapper.Map<Shop, ShopResult>(shopEntity); 
        return result;
    }

    public async Task<ShopResult> UpdateAsync(string id, UpdateShopRequest request)
    {
        var shopToUpdate = await _shopRepository.GetByIdAsync(id);

        if (shopToUpdate == null)
            throw new NotFoundException($"Shop with such id {id} not found");

        _mapper.Map(request, shopToUpdate);
        await _shopRepository.UpdateAsync(shopToUpdate);
        _logger.LogInformation("Shop {@Entity} was updated successfully", shopToUpdate);
        var result = _mapper.Map<Shop, ShopResult>(shopToUpdate); 
        return result;
    }

    public async Task DeleteAsync(string id)
    {
        var shopToDelete = await _shopRepository.GetByIdAsync(id);

        if (shopToDelete == null)
            throw new NotFoundException($"Shop with such id {id} not found");

        await _shopRepository.DeleteAsync(shopToDelete);
        _logger.LogInformation("Shop with id {ShopId} was deleted successfully", shopToDelete.Id);
    }
    
    private Expression<Func<Shop, bool>>? CreateFilterPredicate(GetShopsRequest request)
    {
        Expression<Func<Shop, bool>>? predicate = null;

        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            Expression<Func<Shop, bool>> searchStringExpression = x => x.Name.Contains(request.SearchString);
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