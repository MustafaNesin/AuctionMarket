using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Client.Application.Validators;
using AuctionMarket.Client.Domain.Commands;
using AuctionMarket.Client.Domain.Queries;
using AuctionMarket.Shared.Domain.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace AuctionMarket.Client.Pages;

public partial class AuctionDetailPage
{
    private readonly BidCommand _bidCommand = new();
    private readonly BidCommandValidator _bidValidator = new();
    private AuctionDto _auction = default!;
    private MudForm _bidForm = default!;
    private bool _isSubmitBidDisabled;
    private double _minBidValue;
    private string? _userName;
    private int _watchCount;
    
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

    [Parameter]
    public int Id { get; set; }
    
    [Inject]
    private IHubConnectionService HubConnection { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        var authState = await AuthenticationStateTask;
        _userName = authState.User.Identity?.Name;
        
        var (isSuccess, auction, problemDetails) = await Mediator.Send(new GetAuctionQuery(Id));

        if (isSuccess)
        {
            _auction = auction;
            _auction.CreatedAt = _auction.CreatedAt!.Value.ToLocalTime();
            _auction.StartsAt = _auction.StartsAt!.Value.ToLocalTime();
            _auction.EndsAt = _auction.EndsAt!.Value.ToLocalTime();

            foreach (var bid in _auction.Bids)
                bid.CreatedAt = bid.CreatedAt!.Value.ToLocalTime();
            
            _bidCommand.AuctionId = Id;
            
            var lastBid = auction.Bids.FirstOrDefault();
            _minBidValue = Math.Ceiling(lastBid is null
                ? auction.StartingPrice!.Value
                : lastBid.Value!.Value + lastBid.Value.Value * auction.MinBidIncrement!.Value / 100.0);
            _bidCommand.Value = _minBidValue;
            
            HubConnection.AuctionWatchReceived += OnAuctionWatchReceived;
            HubConnection.BidReceived += OnBidReceived;
            await HubConnection.WatchAuctionAsync(Id);
        }
        else
        {
            ShowProblem(problemDetails, false);
            NavigationManager.NavigateTo("/Auction/List");
        }
    }

    private void OnAuctionWatchReceived(int auctionId, int watchCount)
    {
        if (auctionId != Id)
            return;

        _watchCount = watchCount;
        StateHasChanged();
    }

    private void OnBidReceived(BidDto bid)
    {
        if (bid.AuctionId != Id)
            return;
        
        _minBidValue = Math.Ceiling(bid.Value!.Value + bid.Value.Value * _auction.MinBidIncrement!.Value / 100.0);
        _bidCommand.Value = _minBidValue;
        bid.CreatedAt = bid.CreatedAt!.Value.ToLocalTime();

        _auction.Bids.Insert(0, bid);
        StateHasChanged();
    }

    private async Task OnSubmitBidAsync()
    {
        _isSubmitBidDisabled = true;
        await _bidForm.Validate();

        if (!_bidForm.IsValid)
        {
            _isSubmitBidDisabled = false;
            return;
        }

        var bidValue = _bidCommand.Value;
        var (isSuccess, problemDetails) = await Mediator.Send(_bidCommand);

        if (isSuccess)
            Snackbar.Add($"A successful bid of {bidValue:N2}$ has been placed in the auction.",
                Severity.Success);
        else
            ShowProblem(problemDetails, true);

        _isSubmitBidDisabled = false;
    }

    public override async ValueTask DisposeAsync()
    {
        HubConnection.AuctionWatchReceived -= OnAuctionWatchReceived;
        HubConnection.BidReceived -= OnBidReceived;
        await HubConnection.UnwatchAuctionAsync(Id);
        await base.DisposeAsync();
    }
}