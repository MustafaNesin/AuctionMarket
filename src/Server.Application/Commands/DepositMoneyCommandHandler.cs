using System.Net;
using AuctionMarket.Server.Application.Abstractions;
using AuctionMarket.Server.Application.Hubs;
using AuctionMarket.Server.Domain.Commands;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AuctionMarket.Server.Application.Commands;

public class DepositMoneyCommandHandler : IRequestHandler<DepositMoneyCommand>
{
    private readonly ICurrentAccountService _currentAccount;
    private readonly IAppDbContext _dbContext;
    private readonly IHubContext<AppHub> _hubContext;

    public DepositMoneyCommandHandler(IAppDbContext dbContext, ICurrentAccountService currentAccount,
        IHubContext<AppHub> hubContext)
        => (_dbContext, _currentAccount, _hubContext) = (dbContext, currentAccount, hubContext);

    public async Task<Unit> Handle(DepositMoneyCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentAccount.GetId()!.Value;
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null || user.IsDeleted)
            throw new ProblemDetailsException((int)HttpStatusCode.BadRequest, "User not found.");

        user.Balance += command.Value;
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _hubContext.Clients.User(userId.ToString()).SendAsync("UpdateBalance", user.Balance, cancellationToken);

        return Unit.Value;
    }
}