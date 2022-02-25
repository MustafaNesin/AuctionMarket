// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace AuctionMarket.Server.Domain.Abstractions;

public abstract class AuditableEntity<TId> : AuditableEntity, IEntity<TId>
{
    public TId Id { get; protected set; } = default!;
}