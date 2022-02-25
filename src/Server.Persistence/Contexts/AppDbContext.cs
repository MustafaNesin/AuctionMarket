using AuctionMarket.Server.Application.Abstractions;
using AuctionMarket.Server.Domain.Abstractions;
using AuctionMarket.Server.Domain.Entities;
using AuctionMarket.Server.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AuctionMarket.Server.Persistence.Contexts;

/// <inheritdoc cref="DbContext" />
public class AppDbContext : DbContext, IAppDbContext
{
    private readonly ICurrentAccountService _currentAccount;

    /// <inheritdoc />
    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentAccountService currentAccount) : base(options)
        => _currentAccount = currentAccount;

    public DbSet<Auction> Auctions => Set<Auction>();
    public DbSet<Bid> Bids => Set<Bid>();
    public DbSet<User> Users => Set<User>();

    /// <inheritdoc cref="DbContext.SaveChangesAsync(System.Threading.CancellationToken)" />
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            var now = DateTime.UtcNow;
            var accountId = _currentAccount.GetId();

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedById = accountId;
                    entry.Entity.CreatedAt = now;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedById = accountId;
                    entry.Entity.UpdatedAt = now;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.DeletedById = accountId;
                    entry.Entity.DeletedAt = now;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> SaveSeedChangesAsync(CancellationToken cancellationToken = default)
        => await base.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
}