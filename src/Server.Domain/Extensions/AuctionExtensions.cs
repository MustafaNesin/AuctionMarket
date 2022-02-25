using AuctionMarket.Server.Domain.Entities;
using AuctionMarket.Shared.Domain.Enumerations;
using SharedExtensions = AuctionMarket.Shared.Domain.Extensions.AuctionExtensions;

namespace AuctionMarket.Server.Domain.Extensions;

public static class AuctionExtensions
{
    public static AuctionStatus GetStatus(this Auction auction)
        => SharedExtensions.GetStatus(auction.StartsAt, auction.EndsAt, DateTime.UtcNow);
}