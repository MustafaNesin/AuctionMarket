using AuctionMarket.Shared.Domain.Abstractions;

namespace AuctionMarket.Shared.Domain.DTOs;

public record AuctionDto : AuditableDto, IDto<int?>
{
    public DateTime? EndsAt { get; set; }
    public DateTime? StartsAt { get; set; }

    public IList<BidDto> Bids { get; set; } = new List<BidDto>();

    public int? Id { get; set; }

    #region Mutable

    public string? Description { get; set; }
    public double? MinBidIncrement { get; set; }
    public double? StartingPrice { get; set; }
    public string? Title { get; set; }

    #endregion
}