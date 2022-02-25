using System.Security.Claims;
using AuctionMarket.Client.Application.Abstractions;
using Microsoft.AspNetCore.Components.Authorization;

namespace AuctionMarket.Client.Infrastructure.Services;

public class HostAuthenticationStateProvider : AuthenticationStateProvider, IHostAuthenticationStateProvider
{
    private static readonly Task<AuthenticationState> AnonymousAuthenticationStateTask =
        Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    private Task<AuthenticationState> _authenticationStateTask = AnonymousAuthenticationStateTask;

    public void SetAuthenticationState(Task<AuthenticationState> authenticationStateTask)
    {
        _authenticationStateTask = authenticationStateTask;
        NotifyAuthenticationStateChanged(_authenticationStateTask);
    }

    public void ResetAuthenticationState() => SetAuthenticationState(AnonymousAuthenticationStateTask);

    public override Task<AuthenticationState> GetAuthenticationStateAsync() => _authenticationStateTask;
}