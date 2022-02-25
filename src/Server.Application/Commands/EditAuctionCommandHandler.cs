using System.Net;
using AuctionMarket.Server.Application.Abstractions;
using AuctionMarket.Server.Domain.Commands;
using AuctionMarket.Server.Domain.Extensions;
using AuctionMarket.Shared.Domain.Enumerations;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionMarket.Server.Application.Commands;

public class EditAuctionCommandHandler : IRequestHandler<EditAuctionCommand>
{
    private readonly ICurrentAccountService _currentAccount;
    private readonly IAppDbContext _dbContext;

    public EditAuctionCommandHandler(ICurrentAccountService currentAccount, IAppDbContext dbContext)
        => (_currentAccount, _dbContext) = (currentAccount, dbContext);

    public async Task<Unit> Handle(EditAuctionCommand command, CancellationToken cancellationToken)
    {
        var auction = await _dbContext.Auctions.SingleOrDefaultAsync(
            a => a.Id == command.Auction.Id, cancellationToken);

        if (auction is null || auction.IsDeleted)
            throw new ProblemDetailsException((int)HttpStatusCode.BadRequest, "Auction not found.");

        if (auction.CreatedById != _currentAccount.GetId())
            throw new ProblemDetailsException((int)HttpStatusCode.Unauthorized,
                "You cannot edit someone else's auction.");

        var auctionStatus = auction.GetStatus();

        if (auctionStatus != AuctionStatus.NotStarted)
        {
            if (command.Auction.StartingPrice.HasValue || command.Auction.MinBidIncrement.HasValue)
                throw new ProblemDetailsException((int)HttpStatusCode.BadRequest,
                    "You cannot edit the starting price or the minimum bid increment value after the auction started.");

            if (command.Auction.StartsAt.HasValue || command.Auction.EndsAt.HasValue)
                throw new ProblemDetailsException((int)HttpStatusCode.BadRequest,
                    "You cannot edit the auction start date or the auction end date after the auction started.");
        }
        else
        {
            if (command.Auction.StartingPrice is null or <= 0)
                throw new ProblemDetailsException((int)HttpStatusCode.BadRequest,
                    "Auction starting price must be a positive value.");

            if (command.Auction.MinBidIncrement is null or <= 0)
                throw new ProblemDetailsException((int)HttpStatusCode.BadRequest,
                    "Minimum bid increment value must be a positive value.");

            if (!command.Auction.StartsAt.HasValue || command.Auction.StartsAt.Value <= DateTime.UtcNow)
                throw new ProblemDetailsException((int)HttpStatusCode.BadRequest,
                    "Auction start date must be in the future.");

            if (!command.Auction.EndsAt.HasValue || command.Auction.EndsAt.Value <= command.Auction.StartsAt.Value)
                throw new ProblemDetailsException((int)HttpStatusCode.BadRequest,
                    "Auction end date must be after the start date.");
            
            auction.MinBidIncrement = command.Auction.MinBidIncrement.Value;
            auction.StartingPrice = command.Auction.StartingPrice.Value;
            auction.StartsAt = command.Auction.StartsAt.Value;
            auction.EndsAt = command.Auction.EndsAt.Value;
        }

        auction.Title = command.Auction.Title!;
        auction.Description = command.Auction.Description;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}