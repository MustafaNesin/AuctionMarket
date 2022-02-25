using AuctionMarket.Shared.Domain.DTOs;

namespace AuctionMarket.Shared.Domain.Abstractions.Commands;

public abstract class CreateAuctionCommandBase
{
    public AuctionDto Auction { get; set; } = new();
}