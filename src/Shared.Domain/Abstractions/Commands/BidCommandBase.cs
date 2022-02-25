namespace AuctionMarket.Shared.Domain.Abstractions.Commands;

public abstract class BidCommandBase
{
    public int AuctionId { get; set; }
    public double Value { get; set; }
}