using AuctionMarket.Shared.Domain.Abstractions;

namespace AuctionMarket.Shared.Domain.DTOs;

public record BidDto : AuditableDto, IDto<Guid?>
{
    public int? AuctionId { get; set; }

    public double? Value { get; set; }

    // TODO: Gereksiz ise kaldır
    public Guid? Id { get; set; }
}