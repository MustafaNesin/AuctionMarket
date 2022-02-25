using AuctionMarket.Shared.Domain.Abstractions.Commands;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;

namespace AuctionMarket.Server.Domain.Commands;

public class LoginAccountCommand : LoginAccountCommandBase, IRequest<AccountDto>
{
}