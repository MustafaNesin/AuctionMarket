namespace AuctionMarket.Server.Domain.Abstractions;

public interface IEntity<out TId> : IEntity
{
    TId Id { get; }
}