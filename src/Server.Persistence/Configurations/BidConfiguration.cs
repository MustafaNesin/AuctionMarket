using AuctionMarket.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionMarket.Server.Persistence.Configurations;

public class BidConfiguration : AuditableEntityConfiguration<Bid, Guid>
{
    public override void Configure(EntityTypeBuilder<Bid> builder)
    {
        base.Configure(builder);

        builder.HasOne(bid => bid.Auction)
            .WithMany(auction => auction.Bids)
            .HasForeignKey(bid => bid.AuctionId);

        builder.HasOne(bid => bid.CreatedBy)
            .WithMany(user => user.Bids)
            .HasForeignKey(bid => bid.CreatedById);
    }
}