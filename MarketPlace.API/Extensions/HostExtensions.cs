using MarketPlace.Application.Interfaces.Infrastructure;

namespace MarketPlace.API.Extensions;

public static class HostExtensions
{
    public static IHost SetupIdentity(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<IIdentityInitializer>();
        initializer.InitializeIdentityData();

        return host;
    }
}