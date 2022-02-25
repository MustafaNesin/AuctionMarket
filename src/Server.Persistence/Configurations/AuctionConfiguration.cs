using AuctionMarket.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionMarket.Server.Persistence.Configurations;

public class AuctionConfiguration : AuditableEntityConfiguration<Auction, int>
{
    public override void Configure(EntityTypeBuilder<Auction> builder)
    {
        base.Configure(builder);

        builder.HasOne(auction => auction.CreatedBy)
            .WithMany(user => user.Auctions)
            .HasForeignKey(auction => auction.CreatedById);
    }
}