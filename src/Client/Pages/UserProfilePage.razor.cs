using AuctionMarket.Client.Domain.Queries;
using AuctionMarket.Client.Shared;
using AuctionMarket.Shared.Domain.DTOs;
using Microsoft.AspNetCore.Components;

namespace AuctionMarket.Client.Pages;

public partial class UserProfilePage
{
    private AuctionListComponent _assets = default!;
    private AuctionListComponent _auctions = default!;
    private AuctionListComponent _bids = default!;
    private int? _totalAssets, _totalAuctions, _totalBids;
    private UserDto _user = default!;

    [Parameter]
    public string UserName { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        var (isSuccess, user, problemDetails) = await Mediator.Send(new GetUserQuery(UserName));

        if (isSuccess)
        {
            _user = user;
            _user.CreatedAt = _user.CreatedAt!.Value.ToLocalTime();
        }
        else
        {
            ShowProblem(problemDetails, false);
            NavigationManager.NavigateTo("/");
        }
    }

    public void OnTotalAssetsChanged(int count)
    {
        _totalAssets = count;
        StateHasChanged();
    }

    public void OnTotalAuctionsChanged(int count)
    {
        _totalAuctions = count;
        StateHasChanged();
    }

    public void OnTotalBidsChanged(int count)
    {
        _totalBids = count;
        StateHasChanged();
    }
}