using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Client.Domain.Commands;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AuctionMarket.Client.Pages;

public partial class LogoutAccountPage
{
    private readonly LogoutAccountCommand _command = LogoutAccountCommand.Unit;

    [Inject]
    private IHubConnectionService HubConnection { get; set; } = default!;


    protected override async Task OnInitializedAsync()
    {
        var (isSuccess, problemDetails) = await Mediator.Send(_command);

        if (isSuccess)
        {
            await HubConnection.StopAsync();
            StateContainer.Authentication.ResetAuthenticationState();
            StateContainer.User = default!;
            Snackbar.Add("Logged out successfully!", Severity.Success);
        }
        else
            ShowProblem(problemDetails, false);
    }
}