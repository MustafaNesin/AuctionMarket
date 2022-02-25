using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Client.Domain.Queries;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;

namespace AuctionMarket.Client.Application.Queries;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Response<UserDto>>
{
    private readonly IHttpClientService _httpClient;

    public GetUserQueryHandler(IHttpClientService httpClient)
        => _httpClient = httpClient;

    public async Task<Response<UserDto>> Handle(
        GetUserQuery query, CancellationToken cancellationToken)
        => await _httpClient.GetAsync<UserDto>("Api/User/Get?name=" + query.Name, cancellationToken);
}