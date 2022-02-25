using AuctionMarket.Shared.Domain.Abstractions.Queries;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;

namespace AuctionMarket.Client.Domain.Queries;

public record GetAccountQuery : GetAccountQueryBase, IRequest<Response<AccountDto>>
{
    public static readonly GetAccountQuery Unit = new();
}