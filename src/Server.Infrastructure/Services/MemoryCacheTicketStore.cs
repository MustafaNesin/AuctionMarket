using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;

namespace AuctionMarket.Server.Infrastructure.Services;

public class MemoryCacheTicketStore : ITicketStore
{
    private const string KeyPrefix = "AuthSessionStore-";
    private readonly IMemoryCache _cache;

    public MemoryCacheTicketStore(IMemoryCache cache) => _cache = cache;

    public async Task<string> StoreAsync(AuthenticationTicket ticket)
    {
        var key = KeyPrefix + Guid.NewGuid();

        await RenewAsync(key, ticket);

        return key;
    }

    public Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        var options = new MemoryCacheEntryOptions();
        var expiresUtc = ticket.Properties.ExpiresUtc;

        if (expiresUtc.HasValue)
            options.SetAbsoluteExpiration(expiresUtc.Value);

        options.SetSlidingExpiration(TimeSpan.FromHours(1)); // TODO: Değiştirilebilir
        _cache.Set(key, ticket, options);

        return Task.CompletedTask;
    }

    public Task<AuthenticationTicket?> RetrieveAsync(string key)
        => Task.FromResult(_cache.TryGetValue(key, out AuthenticationTicket? ticket) ? ticket : default);

    public Task RemoveAsync(string key)
    {
        _cache.Remove(key);
        return Task.CompletedTask;
    }
}