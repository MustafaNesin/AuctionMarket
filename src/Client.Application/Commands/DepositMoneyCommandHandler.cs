using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Client.Domain.Commands;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;

namespace AuctionMarket.Client.Application.Commands;

public class DepositMoneyCommandHandler : IRequestHandler<DepositMoneyCommand, Response>
{
    private readonly IHttpClientService _httpClient;

    public DepositMoneyCommandHandler(IHttpClientService httpClient)
        => _httpClient = httpClient;

    public async Task<Response> Handle(DepositMoneyCommand command, CancellationToken cancellationToken)
        => await _httpClient.PostAsync("Api/User/Deposit", command, cancellationToken);
}