using System.Net;
using AuctionMarket.Server.Application.Abstractions;
using AuctionMarket.Server.Domain.Queries;
using AuctionMarket.Shared.Domain.DTOs;
using AutoMapper;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionMarket.Server.Application.Queries;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly ICurrentAccountService _currentAccount;
    private readonly IAppDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(ICurrentAccountService currentAccount, IAppDbContext dbContext, IMapper mapper)
        => (_currentAccount, _dbContext, _mapper) = (currentAccount, dbContext, mapper);

    public async Task<UserDto> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == query.Name,
            cancellationToken);

        if (user is null || user.IsDeleted)
            throw new ProblemDetailsException((int)HttpStatusCode.BadRequest, "User not found.");

        var userDto = _mapper.Map<UserDto>(user);

        if (user.Id != _currentAccount.GetId())
            userDto.Balance = null;

        return userDto;
    }
}