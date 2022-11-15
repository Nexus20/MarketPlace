using Microsoft.AspNetCore.Identity;

namespace MarketPlace.Infrastructure.Identity;

public class AppUser : IdentityUser
{
    public virtual List<AppUserRole>? UserRoles { get; set; }
}