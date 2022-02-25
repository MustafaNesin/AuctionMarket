using AuctionMarket.Shared.Domain.Abstractions.Queries;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;

namespace AuctionMarket.Server.Domain.Queries;

public record GetAccountQuery : GetAccountQueryBase, IRequest<AccountDto>
{
    public static readonly GetAccountQuery Unit = new();
}