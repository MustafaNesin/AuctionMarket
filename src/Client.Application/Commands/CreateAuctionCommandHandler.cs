using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Client.Domain.Commands;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;

namespace AuctionMarket.Client.Application.Commands;

public class CreateAuctionCommandHandler : IRequestHandler<CreateAuctionCommand, Response<int>>
{
    private readonly IHttpClientService _httpClient;

    public CreateAuctionCommandHandler(IHttpClientService httpClient)
        => _httpClient = httpClient;

    public async Task<Response<int>> Handle(CreateAuctionCommand command, CancellationToken cancellationToken) 
        => await _httpClient.PostAsync<CreateAuctionCommand, int>("Api/Auction/Create", command, cancellationToken);
}