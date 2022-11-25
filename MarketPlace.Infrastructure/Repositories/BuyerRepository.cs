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

public class BuyerRepository : RepositoryBase<Buyer>, IBuyerRepository
{
    private readonly ILogger<BuyerRepository> _logger;
    private readonly UserManager<AppUser> _userManager;
    
    public BuyerRepository(ApplicationDbContext dbContext, IMapper mapper, ILogger<BuyerRepository> logger, UserManager<AppUser> userManager) : base(dbContext, mapper)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task AddAsync(Buyer buyerEntity, string password)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync();

        try
        {
            var userEntity = buyerEntity.User;
            await DbContext.DomainUsers.AddAsync(userEntity);
            await DbContext.Buyers.AddAsync(buyerEntity);
            
            var appUser = Mapper.Map<User, AppUser>(userEntity);

            var identityResult = await _userManager.CreateAsync(appUser, password);

            if (!identityResult.Succeeded)
                throw new IdentityException("Error while creating doctor account");

            identityResult = await _userManager.AddToRolesAsync(appUser, new List<string>()
            {
                CustomRoles.User,
                CustomRoles.Buyer,
            });

            if (!identityResult.Succeeded)
                throw new IdentityException("Error while adjusting doctor account roles");
            
            await DbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            _logger.LogError("Error while creating a new buyer account: {EMessage}", e.Message);
        }
    }
}