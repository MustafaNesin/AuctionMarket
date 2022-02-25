namespace AuctionMarket.Server.Application.Abstractions;

public interface IDataSeeder
{
    Task EnsureSeedsAsync(CancellationToken cancellationToken = default);
}