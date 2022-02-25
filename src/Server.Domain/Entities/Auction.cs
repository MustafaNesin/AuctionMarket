using AuctionMarket.Server.Domain.Abstractions;

namespace AuctionMarket.Server.Domain.Entities;

// TODO: Görseller eklenebilir
public sealed class Auction : AuditableEntity<int>
{
    public string? Description { get; set; }
    public DateTime EndsAt { get; set; }
    public double MinBidIncrement { get; set; } // percentage
    public double StartingPrice { get; set; }
    public DateTime StartsAt { get; set; }
    public string Title { get; set; } = default!;

    public ICollection<Bid> Bids { get; set; } = default!;
}