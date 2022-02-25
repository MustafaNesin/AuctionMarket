using AuctionMarket.Server.Domain.DTOs;
using AuctionMarket.Shared.Domain.Abstractions.Queries;
using AuctionMarket.Shared.Domain.DTOs;
using AuctionMarket.Shared.Domain.Enumerations;
using MediatR;

namespace AuctionMarket.Server.Domain.Queries;

public record GetAuctionListQuery(AuctionStatus? Status, string? CreatorName, string? BidderName, string? WinnerName,
        string? SearchQuery, TableState TableState)
    : GetAuctionListQueryBase(Status, CreatorName, BidderName, WinnerName, SearchQuery),
        IRequest<TableData<AuctionDto>>;