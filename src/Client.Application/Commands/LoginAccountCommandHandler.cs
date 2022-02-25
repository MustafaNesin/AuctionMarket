using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Client.Domain.Commands;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;

namespace AuctionMarket.Client.Application.Commands;

public class LoginAccountCommandHandler : IRequestHandler<LoginAccountCommand, Response<AccountDto>>
{
    private readonly IHttpClientService _httpClient;

    public LoginAccountCommandHandler(IHttpClientService httpClient)
        => _httpClient = httpClient;

    public async Task<Response<AccountDto>> Handle(
        LoginAccountCommand command, CancellationToken cancellationToken)
        => await _httpClient.PostAsync<LoginAccountCommand, AccountDto>(
            "Api/Account/Login", command, cancellationToken);
}