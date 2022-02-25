using System.Security.Claims;
using AuctionMarket.Shared.Domain.DTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace AuctionMarket.Client.Infrastructure.Extensions;

public static class AccountDtoExtensions
{
    // TODO: Bunun yerine mapper kullanılabilir
    public static AuthenticationState ConvertToAuthenticationState(this AccountDto account)
    {
        var claims = account.Claims.Select(c => new Claim(c.Key, c.Value));
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "Password")));
    }
}