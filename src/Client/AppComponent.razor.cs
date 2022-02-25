using System.Net;
using System.Security.Claims;
using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Client.Domain.Queries;
using AuctionMarket.Client.Infrastructure.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AuctionMarket.Client;

public partial class AppComponent
{
    [Inject]
    private IHubConnectionService HubConnection { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var authStateTask = FetchAuthenticationStateAsync();
        StateContainer.Authentication.SetAuthenticationState(authStateTask);

        HubConnection.BalanceUpdated += OnBalanceUpdated;

        var authState = await authStateTask;

        if (authState.User.Identity?.IsAuthenticated == true)
        {
            var (isSuccess, user, problemDetails) =
                await Mediator.Send(new GetUserQuery(authState.User.Identity!.Name!));

            if (isSuccess)
            {
                StateContainer.User = user;
                await HubConnection.StartAsync();
            }
            else
            {
                ShowProblem(problemDetails, false);
                NavigationManager.NavigateTo("/Account/Logout");
            }
        }
    }

    private async Task<AuthenticationState> FetchAuthenticationStateAsync()
    {
        var (isSuccess, account, problemDetails) = await Mediator.Send(GetAccountQuery.Unit);

        if (isSuccess)
            return account.ConvertToAuthenticationState();

        if (problemDetails.Status != (int)HttpStatusCode.Unauthorized)
            ShowProblem(problemDetails, true);

        // Return authentication state for anonymous user
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    private void OnBalanceUpdated(double balance)
        => StateContainer.User = StateContainer.User with { Balance = balance };

    public override ValueTask DisposeAsync()
    {
        HubConnection.BalanceUpdated -= OnBalanceUpdated;
        return base.DisposeAsync();
    }
}