using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Client.Domain.Queries;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;

namespace AuctionMarket.Client.Application.Queries;

public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, Response<AccountDto>>
{
    private readonly IHttpClientService _httpClient;

    public GetAccountQueryHandler(IHttpClientService httpClient)
        => _httpClient = httpClient;

    public async Task<Response<AccountDto>> Handle(
        GetAccountQuery query, CancellationToken cancellationToken)
        => await _httpClient.GetAsync<AccountDto>("Api/Account/Get", cancellationToken);
}