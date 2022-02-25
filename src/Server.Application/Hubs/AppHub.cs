using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AuctionMarket.Server.Application.Hubs;

[Authorize]
public class AppHub : Hub
{
    public static readonly AuctionUserDictionary AuctionWatchers = new();

    public async Task WatchAuction(int auctionId)
    {
        var groupName = "Auction" + auctionId;
        var connectionId = Context.ConnectionId;
        var userId = Context.UserIdentifier!;

        var watches = AuctionWatchers.AddOrUpdate(auctionId,
            new UserConnectionDictionary(userId, connectionId),
            (_, userConnections) =>
            {
                userConnections.AddOrUpdate(userId,
                    new ConnectionDictionary(connectionId),
                    (_, connections) =>
                    {
                        connections.TryAdd(connectionId, default);
                        return connections;
                    });
                return userConnections;
            });

        var watchCount = watches.Count(watch => watch.Value.Any());
        await Groups.AddToGroupAsync(connectionId, groupName);
        await Clients.Group(groupName).SendAsync("ReceiveAuctionWatch", auctionId, watchCount);
    }

    public async Task UnwatchAuction(int auctionId)
    {
        var groupName = "Auction" + auctionId;
        var connectionId = Context.ConnectionId;
        var userId = Context.UserIdentifier!;

        var watches = AuctionWatchers.AddOrUpdate(auctionId,
            new UserConnectionDictionary(),
            (_, userConnections) =>
            {
                userConnections.AddOrUpdate(userId,
                    new ConnectionDictionary(),
                    (_, connections) =>
                    {
                        connections.TryRemove(connectionId, out var _);
                        return connections;
                    });
                return userConnections;
            });

        var watchCount = watches.Count(watch => watch.Value.Any());
        await Groups.RemoveFromGroupAsync(connectionId, groupName);
        await Clients.Group(groupName).SendAsync("ReceiveAuctionWatch", auctionId, watchCount);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        var userId = Context.UserIdentifier!;

        foreach (var (auctionId, userConnections) in AuctionWatchers)
        {
            var groupName = "Auction" + auctionId;
            var isConnectionRemoved = false;

            userConnections.AddOrUpdate(userId,
                new ConnectionDictionary(),
                (_, connections) =>
                {
                    isConnectionRemoved = connections.TryRemove(connectionId, out var _);
                    return connections;
                });

            if (!isConnectionRemoved)
                continue;

            var watchCount = userConnections.Count(watch => watch.Value.Any());
            await Clients.Group(groupName).SendAsync("ReceiveAuctionWatch", auctionId, watchCount);
        }
    }

    public class AuctionUserDictionary : ConcurrentDictionary<int, UserConnectionDictionary>
    {
    }

    public class UserConnectionDictionary : ConcurrentDictionary<string, ConnectionDictionary>
    {
        public UserConnectionDictionary()
        {
        }

        public UserConnectionDictionary(string key, string value) :
            base(new List<KeyValuePair<string, ConnectionDictionary>> { new(key, new ConnectionDictionary(value)) })
        {
        }
    }

    public class ConnectionDictionary : ConcurrentDictionary<string, byte>
    {
        public ConnectionDictionary()
        {
        }

        public ConnectionDictionary(string key) : base(new List<KeyValuePair<string, byte>> { new(key, default) })
        {
        }
    }
}