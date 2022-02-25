using Microsoft.AspNetCore.Components.Authorization;

namespace AuctionMarket.Client.Application.Abstractions;

public interface IHostAuthenticationStateProvider
{
    void SetAuthenticationState(Task<AuthenticationState> authenticationStateTask);
    void ResetAuthenticationState();
}