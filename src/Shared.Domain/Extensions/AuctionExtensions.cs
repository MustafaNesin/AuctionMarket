using AuctionMarket.Shared.Domain.DTOs;
using AuctionMarket.Shared.Domain.Enumerations;

namespace AuctionMarket.Shared.Domain.Extensions;

public static class AuctionExtensions
{
    public static AuctionStatus GetStatus(this AuctionDto auction, DateTime now)
        => GetStatus(auction.StartsAt, auction.EndsAt, now);

    public static AuctionStatus GetStatus(DateTime? startsAt, DateTime? endsAt, DateTime now)
    {
        if (startsAt is null || endsAt is null)
            return AuctionStatus.Unknown;

        if (startsAt.Value > now)
            return AuctionStatus.NotStarted;

        return endsAt.Value < now ? AuctionStatus.Ended : AuctionStatus.Active;
    }
}