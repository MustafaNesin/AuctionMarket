using System.Web;
using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Client.Domain.Queries;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;
using MudBlazor;

namespace AuctionMarket.Client.Application.Queries;

public class GetAuctionListQueryHandler : IRequestHandler<GetAuctionListQuery, Response<TableData<AuctionDto>>>
{
    private readonly IHttpClientService _httpClient;

    public GetAuctionListQueryHandler(IHttpClientService httpClient)
        => _httpClient = httpClient;

    public async Task<Response<TableData<AuctionDto>>> Handle(
        GetAuctionListQuery query, CancellationToken cancellationToken)
    {
        var queryString = HttpUtility.ParseQueryString(string.Empty);

        var (status, creatorName, bidderName, winnerName, searchQuery, tableState) = query;
        const string t = nameof(GetAuctionListQuery.TableState) + ".";

        if (status is not null)
            queryString.Add(nameof(GetAuctionListQuery.Status), ((int)status).ToString());

        if (creatorName is not null)
            queryString.Add(nameof(GetAuctionListQuery.CreatorName), creatorName);

        if (bidderName is not null)
            queryString.Add(nameof(GetAuctionListQuery.BidderName), bidderName);

        if (winnerName is not null)
            queryString.Add(nameof(GetAuctionListQuery.WinnerName), winnerName);

        queryString.Add(nameof(GetAuctionListQuery.SearchQuery), searchQuery);
        queryString.Add(t + nameof(GetAuctionListQuery.TableState.Page), tableState.Page.ToString());
        queryString.Add(t + nameof(GetAuctionListQuery.TableState.PageSize), tableState.PageSize.ToString());
        queryString.Add(t + nameof(GetAuctionListQuery.TableState.SortLabel), tableState.SortLabel);
        queryString.Add(t + nameof(GetAuctionListQuery.TableState.SortDirection),
            ((int)tableState.SortDirection).ToString());

        return await _httpClient.GetAsync<TableData<AuctionDto>>(
            "Api/Auction/GetList?" + queryString, cancellationToken);
    }
}