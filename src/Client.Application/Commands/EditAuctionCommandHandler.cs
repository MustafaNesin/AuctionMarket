using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Client.Domain.Commands;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;

namespace AuctionMarket.Client.Application.Commands;

public class EditAuctionCommandHandler : IRequestHandler<EditAuctionCommand, Response>
{
    private readonly IHttpClientService _httpClient;

    public EditAuctionCommandHandler(IHttpClientService httpClient)
        => _httpClient = httpClient;

    public async Task<Response> Handle(EditAuctionCommand command, CancellationToken cancellationToken) 
        => await _httpClient.PostAsync("Api/Auction/Edit", command, cancellationToken);
}