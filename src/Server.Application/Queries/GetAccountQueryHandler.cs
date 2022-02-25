using AuctionMarket.Server.Domain.Queries;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AuctionMarket.Server.Application.Queries;

public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, AccountDto>
{
    private readonly HttpContext _httpContext;

    public GetAccountQueryHandler(IHttpContextAccessor httpContextAccessor)
        => _httpContext = httpContextAccessor.HttpContext!;

    public Task<AccountDto> Handle(GetAccountQuery query, CancellationToken cancellationToken)
        => Task.FromResult(new AccountDto(_httpContext.User.Claims.ToDictionary(c => c.Type, c => c.Value)));
}