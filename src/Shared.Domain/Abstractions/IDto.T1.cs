namespace AuctionMarket.Shared.Domain.Abstractions;

public interface IDto<out TId> : IDto
{
    TId Id { get; }
}