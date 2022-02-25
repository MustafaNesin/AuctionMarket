using AuctionMarket.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuctionMarket.Server.Application.Abstractions;

public interface IAppDbContext
{
    DbSet<Auction> Auctions { get; }
    DbSet<Bid> Bids { get; }
    DbSet<User> Users { get; }

    /// <inheritdoc cref="DbContext.Entry" />
    EntityEntry Entry(object entity);

    /// <inheritdoc cref="DbContext.Entry{TEntity}" />
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    /// <inheritdoc cref="DbContext.SaveChangesAsync(System.Threading.CancellationToken)" />
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<int> SaveSeedChangesAsync(CancellationToken cancellationToken = default);
}