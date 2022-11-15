using Microsoft.AspNetCore.Identity;

namespace MarketPlace.Infrastructure.Identity;

public class AppRole : IdentityRole
{
    public AppRole()
    {
        
    }
    
    public AppRole(string roleName) : base(roleName) { }
    
    public virtual List<AppUserRole>? UserRoles { get; set; }
}