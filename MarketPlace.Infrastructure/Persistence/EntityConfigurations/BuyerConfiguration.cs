using MarketPlace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlace.Infrastructure.Persistence.EntityConfigurations;

internal class BuyerConfiguration : IEntityTypeConfiguration<Buyer>
{
    public void Configure(EntityTypeBuilder<Buyer> builder)
    {
        builder.HasOne(x => x.User)
            .WithOne(x => x.Buyer)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Orders)
            .WithOne(x => x.Buyer)
            .HasForeignKey(x => x.BuyerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}