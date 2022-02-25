using System.Net;
using AuctionMarket.Server.Application.Abstractions;
using AuctionMarket.Server.Application.Hubs;
using AuctionMarket.Server.Domain.Commands;
using AuctionMarket.Server.Domain.Entities;
using AuctionMarket.Server.Domain.Extensions;
using AuctionMarket.Shared.Domain.DTOs;
using AuctionMarket.Shared.Domain.Enumerations;
using AutoMapper;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AuctionMarket.Server.Application.Commands;

public class BidCommandHandler : IRequestHandler<BidCommand>
{
    private readonly ICurrentAccountService _currentAccount;
    private readonly IAppDbContext _dbContext;
    private readonly IHubContext<AppHub> _hubContext;
    private readonly IMapper _mapper;

    public BidCommandHandler(IAppDbContext dbContext, ICurrentAccountService currentAccount,
        IHubContext<AppHub> hubContext, IMapper mapper)
        => (_dbContext, _currentAccount, _hubContext, _mapper) = (dbContext, currentAccount, hubContext, mapper);

    public async Task<Unit> Handle(BidCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentAccount.GetId()!.Value;
        var auction = await _dbContext.Auctions.Include(a => a.Bids).SingleOrDefaultAsync(
            a => a.Id == command.AuctionId, cancellationToken);

        if (auction is null || auction.IsDeleted)
            throw new ProblemDetailsException((int)HttpStatusCode.BadRequest, "Auction not found.");

        if (auction.CreatedById == userId)
            throw new ProblemDetailsException((int)HttpStatusCode.BadRequest, "You cannot bid on your own auction.");

        switch (auction.GetStatus())
        {
            case AuctionStatus.NotStarted:
                throw new ProblemDetailsException((int)HttpStatusCode.BadRequest, "Auction has not started yet.");
            case AuctionStatus.Ended:
                throw new ProblemDetailsException((int)HttpStatusCode.BadRequest, "Auction has ended.");
            case AuctionStatus.Active:
            case AuctionStatus.Unknown:
            default:
                break;
        }

        var user = await _dbContext.Users.SingleOrDefaultAsync(
            u => u.Id == userId, cancellationToken);

        if (user is null || user.IsDeleted)
            throw new ProblemDetailsException((int)HttpStatusCode.BadRequest, "User not found.");

        var creator = await _dbContext.Users.SingleOrDefaultAsync(
            u => u.Id == auction.CreatedById, cancellationToken);

        if (creator is null || creator.IsDeleted)
            throw new ProblemDetailsException((int)HttpStatusCode.BadRequest, "Auction creator not found.");

        var lastBid = auction.Bids.LastOrDefault();
        var lastBidUser = default(User?);

        if (lastBid is not null)
        {
            if (lastBid.CreatedById == userId)
                throw new ProblemDetailsException((int)HttpStatusCode.BadRequest,
                    "You cannot make a new bid on this auction since the last bid is yours.");

            if (command.Value < lastBid.Value + lastBid.Value * auction.MinBidIncrement / 100.0)
                throw new ProblemDetailsException((int)HttpStatusCode.BadRequest,
                    "Bid value is not higher enough than the last bid.");

            lastBidUser = await _dbContext.Users.SingleOrDefaultAsync(
                u => u.Id == lastBid.CreatedById, cancellationToken);

            if (lastBidUser is null || lastBidUser.IsDeleted)
                lastBidUser = null;
            else
                lastBidUser.Balance += lastBid.Value;

            creator.Balance -= lastBid.Value;
        }
        else if (command.Value < auction.StartingPrice)
            throw new ProblemDetailsException((int)HttpStatusCode.BadRequest,
                "Initial bid value cannot be lower than the starting price.");

        user.Balance -= command.Value;
        creator.Balance += command.Value;

        var bid = new Bid
        {
            AuctionId = auction.Id,
            Value = command.Value
        };

        _dbContext.Bids.Add(bid);
        await _dbContext.SaveChangesAsync(cancellationToken);

        if (lastBidUser is not null)
            await _hubContext.Clients.User(lastBidUser.Id.ToString())
                .SendAsync("UpdateBalance", lastBidUser.Balance, cancellationToken);

        await _hubContext.Clients.User(userId.ToString())
            .SendAsync("UpdateBalance", user.Balance, cancellationToken);

        await _hubContext.Clients.User(creator.Id.ToString())
            .SendAsync("UpdateBalance", creator.Balance, cancellationToken);

        var bidDto = _mapper.Map<BidDto>(bid);
        await _hubContext.Clients.Group("Auction" + auction.Id).SendAsync("ReceiveBid", bidDto, cancellationToken);

        return Unit.Value;
    }
}