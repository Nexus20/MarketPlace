using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MarketPlace.Application.Authorization;
using MarketPlace.Application.Interfaces.Persistent;
using MarketPlace.Domain.Entities;
using MarketPlace.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MarketPlace.Infrastructure.Auth;

public class JwtHandler {
    
    private readonly IConfigurationSection _jwtSettings;
    private readonly UserManager<AppUser> _userManager;
    private readonly IRepository<User> _userRepository;

    public JwtHandler(IConfiguration configuration, UserManager<AppUser> userManager, IRepository<User> userRepository) {
        _userManager = userManager;
        _userRepository = userRepository;
        _jwtSettings = configuration.GetSection("JwtSettings");
    }
    
    public SigningCredentials GetSigningCredentials() {
        
        var key = Encoding.UTF8.GetBytes(_jwtSettings["securityKey"]);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
    
    public async Task<List<Claim>> GetClaimsAsync(AppUser user) {
        
        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };
        
        var roles = await _userManager.GetRolesAsync(user);
        
        foreach (var role in roles) {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var domainUser = await _userRepository.GetByIdAsync(user.Id);
        
        if(domainUser?.Shop != null)
            claims.Add(new Claim(CustomClaimTypes.ShopId, domainUser.Shop.Id));
        
        if(domainUser?.Buyer != null)
            claims.Add(new Claim(CustomClaimTypes.BuyerId, domainUser.Buyer.Id));
        
        return claims;
    }
    
    public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims) {
        
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtSettings["validIssuer"],
            audience: _jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
            signingCredentials: signingCredentials);
        
        return tokenOptions;
    }
}