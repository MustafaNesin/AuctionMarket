using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Client.Domain.Commands;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;

namespace AuctionMarket.Client.Application.Commands;

public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand, Response>
{
    private readonly IHttpClientService _httpClient;

    public RegisterAccountCommandHandler(IHttpClientService httpClient)
        => _httpClient = httpClient;

    public async Task<Response> Handle(RegisterAccountCommand command, CancellationToken cancellationToken)
        => await _httpClient.PostAsync("Api/Account/Register", command, cancellationToken);
}