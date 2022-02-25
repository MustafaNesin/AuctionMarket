using AuctionMarket.Shared.Domain.Abstractions.Commands;
using MediatR;

namespace AuctionMarket.Server.Domain.Commands;

public class LogoutAccountCommand : LogoutAccountCommandBase, IRequest
{
    public static readonly LogoutAccountCommand Unit = new();
}