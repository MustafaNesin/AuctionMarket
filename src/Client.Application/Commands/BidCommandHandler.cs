using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Client.Domain.Commands;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;

namespace AuctionMarket.Client.Application.Commands;

public class BidCommandHandler : IRequestHandler<BidCommand, Response>
{
    private readonly IHttpClientService _httpClient;

    public BidCommandHandler(IHttpClientService httpClient)
        => _httpClient = httpClient;

    public async Task<Response> Handle(BidCommand command, CancellationToken cancellationToken)
        => await _httpClient.PostAsync("Api/Bid/Create", command, cancellationToken);
}