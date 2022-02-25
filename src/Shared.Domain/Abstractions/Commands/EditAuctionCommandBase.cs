using AuctionMarket.Shared.Domain.DTOs;

namespace AuctionMarket.Shared.Domain.Abstractions.Commands;

public abstract class EditAuctionCommandBase
{
    public AuctionDto Auction { get; set; } = new();
}