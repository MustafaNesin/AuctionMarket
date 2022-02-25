using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Shared.Domain.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace AuctionMarket.Client.Application.Services;

public class HubConnectionService : IHubConnectionService, IAsyncDisposable
{
    private readonly HubConnection _connection;

    public HubConnectionService(NavigationManager navigationManager)
    {
        var url = navigationManager.ToAbsoluteUri("/Hub");
        _connection = new HubConnectionBuilder().WithUrl(url).Build();

        _connection.On<int, int>("ReceiveAuctionWatch",
            (auctionId, watchCount) => AuctionWatchReceived?.Invoke(auctionId, watchCount));

        _connection.On<BidDto>("ReceiveBid",
            bid => BidReceived?.Invoke(bid));

        _connection.On<double>("UpdateBalance",
            balance => BalanceUpdated?.Invoke(balance));
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public event Action<int, int>? AuctionWatchReceived;
    public event Action<double>? BalanceUpdated;
    public event Action<BidDto>? BidReceived;

    public async Task StartAsync(CancellationToken cancellationToken = default)
        => await _connection.StartAsync(cancellationToken);

    public async Task StopAsync(CancellationToken cancellationToken = default)
        => await _connection.StopAsync(cancellationToken);

    public async Task WatchAuctionAsync(int auctionId, CancellationToken cancellationToken = default)
        => await _connection.SendAsync("WatchAuction", auctionId, cancellationToken);

    public async Task UnwatchAuctionAsync(int auctionId, CancellationToken cancellationToken = default)
        => await _connection.SendAsync("UnwatchAuction", auctionId, cancellationToken);
}