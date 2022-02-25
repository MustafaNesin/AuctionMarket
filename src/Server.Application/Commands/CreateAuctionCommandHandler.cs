using AuctionMarket.Server.Application.Abstractions;
using AuctionMarket.Server.Domain.Commands;
using AuctionMarket.Server.Domain.Entities;
using AutoMapper;
using Hellang.Middleware.ProblemDetails;
using MediatR;

namespace AuctionMarket.Server.Application.Commands;

public class CreateAuctionCommandHandler : IRequestHandler<CreateAuctionCommand, int>
{
    private readonly IAppDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateAuctionCommandHandler(IAppDbContext dbContext, IMapper mapper)
        => (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<int> Handle(CreateAuctionCommand command, CancellationToken cancellationToken)
    {
        command.Auction.CreatedAt = default;
        command.Auction.CreatedBy = default;
        
        var auction = _mapper.Map<Auction>(command.Auction);

        _dbContext.Auctions.Add(auction);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return auction.Id;
    }
}