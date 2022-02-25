using AuctionMarket.Shared.Domain.Abstractions.Commands;
using MediatR;

namespace AuctionMarket.Server.Domain.Commands;

public class EditAuctionCommand : EditAuctionCommandBase, IRequest
{
}