using AuctionMarket.Shared.Domain.Abstractions.Commands;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;

namespace AuctionMarket.Client.Domain.Commands;

public class DepositMoneyCommand : DepositMoneyCommandBase, IRequest<Response>
{
}