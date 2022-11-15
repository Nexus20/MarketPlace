using System.Reflection;
using MarketPlace.Application.Interfaces.Infrastructure;
using MarketPlace.Application.Interfaces.Persistent;
using MarketPlace.Application.Interfaces.Services;
using MarketPlace.Infrastructure.Auth;
using MarketPlace.Infrastructure.Identity;
using MarketPlace.Infrastructure.Persistence;
using MarketPlace.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarketPlace.Infrastructure;

public static class InfrastructureServicesRegistration
{

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddIdentity<AppUser, AppRole>()
            .AddUserStore<UserStore<AppUser, AppRole, ApplicationDbContext, string, IdentityUserClaim<string>, AppUserRole,
                IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>>()
            .AddRoleStore<RoleStore<AppRole, ApplicationDbContext, string, AppUserRole, IdentityRoleClaim<string>>>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddUserManager<UserManager<AppUser>>();

        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));

        services.AddScoped<IIdentityInitializer, IdentityInitializer>();
        services.AddScoped<ISignInService, SignInService>();
        services.AddScoped<JwtHandler>();

        return services;
    }
}