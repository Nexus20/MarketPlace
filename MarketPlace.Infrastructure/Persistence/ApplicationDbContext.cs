using System.Reflection;
using MarketPlace.Domain.Entities;
using MarketPlace.Domain.Entities.Abstract;
using MarketPlace.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, string, IdentityUserClaim<string>, AppUserRole,
    IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public DbSet<User> DomainUsers { get; set; }
    public DbSet<Buyer> Buyers { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductPhoto> ProductPhotos { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductCategory> ProductsCategories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        if (!Database.IsInMemory())
        {
            Database.Migrate();
        }
    }

    public override int SaveChanges()
    {
        AddInfoBeforeUpdate();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        AddInfoBeforeUpdate();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void AddInfoBeforeUpdate()
    {
        var entries = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseEntity && x.State is EntityState.Added or EntityState.Modified);
        
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                ((BaseEntity)entry.Entity).CreatedDate = DateTime.UtcNow;
            }
            ((BaseEntity)entry.Entity).LastModifiedDate = DateTime.UtcNow;
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ApplicationDbContext))!);
    }
}