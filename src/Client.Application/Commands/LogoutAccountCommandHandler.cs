using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Client.Domain.Commands;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;

namespace AuctionMarket.Client.Application.Commands;

public class LogoutAccountCommandHandler : IRequestHandler<LogoutAccountCommand, Response>
{
    private readonly IHttpClientService _httpClient;

    public LogoutAccountCommandHandler(IHttpClientService httpClient)
        => _httpClient = httpClient;

    public async Task<Response> Handle(LogoutAccountCommand command, CancellationToken cancellationToken)
        => await _httpClient.PostAsync("Api/Account/Logout", command, cancellationToken);
}