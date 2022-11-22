using AutoMapper;
using MarketPlace.Application.Authorization;
using MarketPlace.Application.Exceptions;
using MarketPlace.Application.Interfaces.Persistent;
using MarketPlace.Domain.Entities;
using MarketPlace.Infrastructure.Identity;
using MarketPlace.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MarketPlace.Infrastructure.Repositories;

public class ShopRepository : RepositoryBase<Shop>, IShopRepository
{
    private readonly ILogger<ShopRepository> _logger;
    private readonly UserManager<AppUser> _userManager;
    
    public ShopRepository(ApplicationDbContext dbContext, IMapper mapper, ILogger<ShopRepository> logger, UserManager<AppUser> userManager) : base(dbContext, mapper)
    {
        _logger = logger;
        _userManager = userManager;
    }
    
    public async Task AddAsync(Shop shopEntity, string password)
    {
        var userEntity = shopEntity.User;
        await using var transaction = await DbContext.Database.BeginTransactionAsync();
        try
        {
            await DbContext.DomainUsers.AddAsync(userEntity);
            await DbContext.Shops.AddAsync(shopEntity);
            var appUser = Mapper.Map<User, AppUser>(userEntity);

            var identityResult = await _userManager.CreateAsync(appUser, password);

            if (!identityResult.Succeeded)
                throw new IdentityException("Error while creating shop account");

            identityResult = await _userManager.AddToRolesAsync(appUser, new List<string>()
            {
                CustomRoles.User,
                CustomRoles.Shop,
            });

            if (!identityResult.Succeeded)
                throw new IdentityException("Error while adjusting shop account roles");
            
            await DbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            _logger.LogError("Error while creating a new shop request: {EMessage}", e.Message);
        }
    }
}