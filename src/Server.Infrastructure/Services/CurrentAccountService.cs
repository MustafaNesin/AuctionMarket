using System.Security.Claims;
using AuctionMarket.Server.Application.Abstractions;
using AuctionMarket.Server.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace AuctionMarket.Server.Infrastructure.Services;

public class CurrentAccountService : ICurrentAccountService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentAccountService(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public Guid? GetId() => FindFirstGuid(ClaimTypes.NameIdentifier);

    public Guid? GetSecurityStamp() => FindFirstGuid(nameof(User.SecurityStamp));

    private Guid? FindFirstGuid(string claimType)
        => Guid.TryParse(FindFirstValue(claimType), out var guid) ? guid : null;

    private string? FindFirstValue(string claimType)
        => _httpContextAccessor.HttpContext?.User.FindFirstValue(claimType);
}