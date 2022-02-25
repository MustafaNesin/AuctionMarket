using System.Linq.Expressions;
using AuctionMarket.Server.Application.Abstractions;
using AuctionMarket.Server.Domain.DTOs;
using AuctionMarket.Server.Domain.Entities;
using AuctionMarket.Server.Domain.Enumerations;
using AuctionMarket.Server.Domain.Queries;
using AuctionMarket.Shared.Domain.Abstractions.Queries;
using AuctionMarket.Shared.Domain.DTOs;
using AuctionMarket.Shared.Domain.Enumerations;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionMarket.Server.Application.Queries;

public class GetAuctionListQueryHandler : IRequestHandler<GetAuctionListQuery, TableData<AuctionDto>>
{
    private readonly IAppDbContext _dbContext;

    private readonly Expression<Func<Auction, AuctionStatus>> _getAuctionStatusExpression =
        auction => auction.StartsAt > DateTime.UtcNow
            ? AuctionStatus.NotStarted
            : auction.EndsAt > DateTime.UtcNow
                ? AuctionStatus.Active
                : AuctionStatus.Ended;

    private readonly Expression<Func<Auction, DateTime?>> _getLastBidDateExpression =
        a => a.Bids.Count > 0 ? a.Bids.OrderBy(b => b.CreatedAt).Last().CreatedAt : null;

    private readonly Expression<Func<Auction, string?>> _getLastBidderNameExpression =
        a => a.Bids.Count > 0 ? a.Bids.OrderBy(b => b.CreatedAt).Last().CreatedBy!.UserName : null;

    private readonly Expression<Func<Auction, double?>> _getLastBidValueExpression =
        a => a.Bids.Count > 0 ? a.Bids.OrderBy(b => b.CreatedAt).Last().Value : .0;

    private readonly IMapper _mapper;

    public GetAuctionListQueryHandler(IAppDbContext dbContext, IMapper mapper)
        => (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<TableData<AuctionDto>> Handle(GetAuctionListQuery query, CancellationToken cancellationToken)
    {
        var (status, creatorName, bidderName, winnerName, searchQuery, tableState) = query;
        var allAuctionsQuery = _dbContext.Auctions
            .Include(a => a.CreatedBy)
            .Include(a => a.Bids)
            .ThenInclude(b => b.CreatedBy)
            .Where(a => a.DeletedAt == null)
            .Where(a => status != AuctionStatus.NotStarted || a.StartsAt > DateTime.UtcNow)
            .Where(a => status != AuctionStatus.Active || a.StartsAt < DateTime.UtcNow && a.EndsAt > DateTime.UtcNow)
            .Where(a => status != AuctionStatus.Ended || a.EndsAt < DateTime.UtcNow)
            .Where(a => creatorName == null || a.CreatedBy!.UserName == creatorName)
            .Where(a => bidderName == null || a.Bids.Any(b => b.CreatedBy!.UserName == bidderName))
            .Where(a => winnerName == null ||
                        status == AuctionStatus.Ended && a.Bids.Count > 0 &&
                        a.Bids.OrderBy(b => b.CreatedAt).Last().CreatedBy!.UserName == winnerName)
            .Where(a => searchQuery == null || EF.Functions.Like(a.Title + " " + a.Description, $"%{searchQuery}%"));

        #region Sorting

        allAuctionsQuery = tableState.SortLabel switch
        {
            GetAuctionListQueryBase.TitleSortLabel => tableState.SortDirection switch
            {
                SortDirection.Descending => allAuctionsQuery.OrderByDescending(a => a.Title),
                SortDirection.Ascending => allAuctionsQuery.OrderBy(a => a.Title),
                _ => allAuctionsQuery
            },
            GetAuctionListQueryBase.CreatorSortLabel => tableState.SortDirection switch
            {
                SortDirection.Descending => allAuctionsQuery.OrderByDescending(a => a.CreatedBy!.UserName),
                SortDirection.Ascending => allAuctionsQuery.OrderBy(a => a.CreatedBy!.UserName),
                _ => allAuctionsQuery
            },
            GetAuctionListQueryBase.CreatedAtSortLabel => tableState.SortDirection switch
            {
                SortDirection.Descending => allAuctionsQuery.OrderByDescending(a => a.CreatedAt),
                SortDirection.Ascending => allAuctionsQuery.OrderBy(a => a.CreatedAt),
                _ => allAuctionsQuery
            },
            GetAuctionListQueryBase.StartingPriceSortLabel => tableState.SortDirection switch
            {
                SortDirection.Descending => allAuctionsQuery.OrderByDescending(a => a.StartingPrice),
                SortDirection.Ascending => allAuctionsQuery.OrderBy(a => a.StartingPrice),
                _ => allAuctionsQuery
            },
            GetAuctionListQueryBase.MinIncrementSortLabel => tableState.SortDirection switch
            {
                SortDirection.Descending => allAuctionsQuery.OrderByDescending(a => a.MinBidIncrement),
                SortDirection.Ascending => allAuctionsQuery.OrderBy(a => a.MinBidIncrement),
                _ => allAuctionsQuery
            },
            GetAuctionListQueryBase.LastBidderSortLabel => tableState.SortDirection switch
            {
                SortDirection.Descending => allAuctionsQuery.OrderByDescending(_getLastBidderNameExpression),
                SortDirection.Ascending => allAuctionsQuery.OrderBy(_getLastBidderNameExpression),
                _ => allAuctionsQuery
            },
            GetAuctionListQueryBase.LastBidAtSortLabel => tableState.SortDirection switch
            {
                SortDirection.Descending => allAuctionsQuery.OrderByDescending(_getLastBidDateExpression),
                SortDirection.Ascending => allAuctionsQuery.OrderBy(_getLastBidDateExpression),
                _ => allAuctionsQuery
            },
            GetAuctionListQueryBase.LastBidValueSortLabel => tableState.SortDirection switch
            {
                SortDirection.Descending => allAuctionsQuery.OrderByDescending(_getLastBidValueExpression),
                SortDirection.Ascending => allAuctionsQuery.OrderBy(_getLastBidValueExpression),
                _ => allAuctionsQuery
            },
            GetAuctionListQueryBase.StartsAtSortLabel => tableState.SortDirection switch
            {
                SortDirection.Descending => allAuctionsQuery.OrderByDescending(a => a.StartsAt),
                SortDirection.Ascending => allAuctionsQuery.OrderBy(a => a.StartsAt),
                _ => allAuctionsQuery
            },
            GetAuctionListQueryBase.EndsAtSortLabel => tableState.SortDirection switch
            {
                SortDirection.Descending => allAuctionsQuery.OrderByDescending(a => a.EndsAt),
                SortDirection.Ascending => allAuctionsQuery.OrderBy(a => a.EndsAt),
                _ => allAuctionsQuery
            },
            GetAuctionListQueryBase.StatusSortLabel => tableState.SortDirection switch
            {
                SortDirection.Descending => allAuctionsQuery.OrderByDescending(_getAuctionStatusExpression),
                SortDirection.Ascending => allAuctionsQuery.OrderBy(_getAuctionStatusExpression),
                _ => allAuctionsQuery
            },
            _ => allAuctionsQuery
        };

        #endregion

        var auctionCount = await allAuctionsQuery.CountAsync(cancellationToken);

        // TODO: Bu iki sorgu tek sorguda birleştirilebilir
        var auctionsQuery = allAuctionsQuery
            .Skip(tableState.Page * tableState.PageSize)
            .Take(tableState.PageSize);

        var auctions = await _mapper.ProjectTo<AuctionDto>(auctionsQuery).ToListAsync(cancellationToken);

        foreach (var auction in auctions.Where(a => a.Bids.Count > 1))
            auction.Bids = new List<BidDto> { auction.Bids.OrderByDescending(b => b.CreatedAt).First() };

        return new TableData<AuctionDto>(auctions, auctionCount);
    }
}