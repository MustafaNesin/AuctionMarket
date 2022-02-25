using AuctionMarket.Shared.Domain.DTOs;

namespace AuctionMarket.Client.Application.Abstractions;

public interface IHubConnectionService
{
    event Action<int, int>? AuctionWatchReceived;
    event Action<double>? BalanceUpdated;
    event Action<BidDto>? BidReceived;

    Task StartAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
    Task WatchAuctionAsync(int auctionId, CancellationToken cancellationToken = default);
    Task UnwatchAuctionAsync(int auctionId, CancellationToken cancellationToken = default);
}