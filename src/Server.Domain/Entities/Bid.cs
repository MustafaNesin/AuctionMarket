using AuctionMarket.Server.Domain.Abstractions;

namespace AuctionMarket.Server.Domain.Entities;

public sealed class Bid : AuditableEntity<Guid>
{
    public Bid() => Id = Guid.NewGuid();

    public Auction Auction { get; set; } = default!;
    public int AuctionId { get; set; }
    public double Value { get; set; }
}