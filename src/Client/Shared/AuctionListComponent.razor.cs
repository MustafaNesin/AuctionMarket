using AuctionMarket.Client.Domain.Queries;
using AuctionMarket.Shared.Domain.DTOs;
using AuctionMarket.Shared.Domain.Enumerations;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AuctionMarket.Client.Shared;

public partial class AuctionListComponent
{
    private bool _isSearchDisabled;
    private string? _searchQuery;
    private AuctionStatus? _status;
    private MudTable<AuctionDto> _table = default!;

    [Parameter]
    public string? BidderName { get; set; }

    [Parameter]
    public string? CreatorName { get; set; }

    [Parameter]
    public string? WinnerName { get; set; }

    [Parameter]
    public EventCallback<int> TotalItemsChanged { get; set; }

    private AuctionStatus? Status { get; set; }

    protected override Task OnParametersSetAsync()
    {
        if (WinnerName is not null)
            Status = AuctionStatus.Ended;

        return base.OnParametersSetAsync();
    }

    private async Task<TableData<AuctionDto>> OnServerReload(TableState state)
    {
        if (WinnerName is null)
            Status = _status;

        var query = new GetAuctionListQuery(Status, CreatorName, BidderName, WinnerName, _searchQuery, state);

        var (isSuccess, tableData, problemDetails) = await Mediator.Send(query);

        if (isSuccess)
        {
            foreach (var auction in tableData.Items)
            {
                auction.CreatedAt = auction.CreatedAt!.Value.ToLocalTime();
                auction.StartsAt = auction.StartsAt!.Value.ToLocalTime();
                auction.EndsAt = auction.EndsAt!.Value.ToLocalTime();

                foreach (var bid in auction.Bids)
                    bid.CreatedAt = bid.CreatedAt!.Value.ToLocalTime();
            }
            
            await TotalItemsChanged.InvokeAsync(tableData.TotalItems);
            return tableData;
        }

        ShowProblem(problemDetails, false);
        return new TableData<AuctionDto> { Items = Array.Empty<AuctionDto>(), TotalItems = 0 };
    }

    private void OnSearch()
    {
        _isSearchDisabled = true;
        _table.ReloadServerData();
        _isSearchDisabled = false;
    }
}