using MarketPlace.Application.Authorization;
using MarketPlace.Application.Interfaces.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace MarketPlace.Infrastructure.Identity;

public class IdentityInitializer : IIdentityInitializer
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public IdentityInitializer(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void InitializeIdentityData()
    {
        InitializeSuperAdminRole().Wait();
        RegisterRoleAsync(CustomRoles.Admin).Wait();
        RegisterRoleAsync(CustomRoles.Moderator).Wait();
        RegisterRoleAsync(CustomRoles.User).Wait();
        RegisterRoleAsync(CustomRoles.Buyer).Wait();
        RegisterRoleAsync(CustomRoles.Shop).Wait();
    }
    
    private async Task<AppRole> RegisterRoleAsync(string roleName)
    {

        var role  = await _roleManager.FindByNameAsync(roleName);

        if (role != null) {
            return role;
        }

        role = new AppRole(roleName);
        await _roleManager.CreateAsync(role);

        return role;
    }
    
    private async Task InitializeSuperAdminRole() {

        var superAdmin = _userManager.Users.FirstOrDefault(u => u.UserName == "root") ?? RegisterSuperAdmin();
            
        var superAdminRole = await RegisterRoleAsync(CustomRoles.SuperAdmin);

        if(!await _userManager.IsInRoleAsync(superAdmin, CustomRoles.SuperAdmin))
            await _userManager.AddToRoleAsync(superAdmin, superAdminRole.Name);
    }
    
    private AppUser RegisterSuperAdmin() {

        var superAdmin = new AppUser() {
            UserName = "root@marketplace.com",
            Email = "root@marketplace.com"
        };

        _userManager.CreateAsync(superAdmin, "_QGrXyvcmTD4aVQJ_").Wait();

        return superAdmin;
    }
}