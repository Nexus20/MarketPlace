using MarketPlace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlace.Infrastructure.Persistence.EntityConfigurations;

internal class ShopConfiguration : IEntityTypeConfiguration<Shop>
{
    public void Configure(EntityTypeBuilder<Shop> builder)
    {
        builder.HasOne(x => x.User)
            .WithOne(x => x.Shop)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.Orders)
            .WithOne(x => x.Shop)
            .HasForeignKey(x => x.ShopId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.Products)
            .WithOne(x => x.Shop)
            .HasForeignKey(x => x.ShopId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}