using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Client.Domain.Queries;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;

namespace AuctionMarket.Client.Application.Queries;

public class GetAuctionQueryHandler : IRequestHandler<GetAuctionQuery, Response<AuctionDto>>
{
    private readonly IHttpClientService _httpClient;

    public GetAuctionQueryHandler(IHttpClientService httpClient)
        => _httpClient = httpClient;

    public async Task<Response<AuctionDto>> Handle(
        GetAuctionQuery query, CancellationToken cancellationToken)
        => await _httpClient.GetAsync<AuctionDto>("Api/Auction/Get?id=" + query.Id, cancellationToken);
}