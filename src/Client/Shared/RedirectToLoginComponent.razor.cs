using AuctionMarket.Client.Application.Abstractions;
using Microsoft.AspNetCore.Components;

namespace AuctionMarket.Client.Shared;

public partial class RedirectToLoginComponent
{
    [Inject]
    private IHubConnectionService HubConnection { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await HubConnection.StopAsync();
        NavigationManager.NavigateTo("/Account/Login");
    }
}