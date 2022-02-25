using AuctionMarket.Shared.Domain.Abstractions.Queries;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;

namespace AuctionMarket.Server.Domain.Queries;

public record GetAuctionQuery(int Id) : GetAuctionQueryBase(Id), IRequest<AuctionDto>;