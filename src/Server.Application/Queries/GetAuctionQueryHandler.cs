using System.Net;
using AuctionMarket.Server.Application.Abstractions;
using AuctionMarket.Server.Domain.Queries;
using AuctionMarket.Shared.Domain.DTOs;
using AutoMapper;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionMarket.Server.Application.Queries;

public class GetAuctionQueryHandler : IRequestHandler<GetAuctionQuery, AuctionDto>
{
    private readonly IAppDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAuctionQueryHandler(IAppDbContext dbContext, IMapper mapper)
        => (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<AuctionDto> Handle(GetAuctionQuery query, CancellationToken cancellationToken)
    {
        var auction = await _dbContext.Auctions
            .Include(a => a.CreatedBy)
            .Include(a => a.Bids)
            .ThenInclude(b => b.CreatedBy)
            .SingleOrDefaultAsync(a => a.Id == query.Id, cancellationToken);

        if (auction is null || auction.IsDeleted)
            throw new ProblemDetailsException((int)HttpStatusCode.BadRequest, "Auction not found.");

        var auctionDto = _mapper.Map<AuctionDto>(auction);
        auctionDto.Bids = auctionDto.Bids.OrderByDescending(b => b.CreatedAt).ToList();

        return auctionDto;
    }
}