using AuctionMarket.Shared.Domain.DTOs;

namespace AuctionMarket.Shared.Domain.Abstractions;

public abstract record AuditableDto : IDto
{
    public DateTime? CreatedAt { get; set; }
    public UserDto? CreatedBy { get; set; }
}