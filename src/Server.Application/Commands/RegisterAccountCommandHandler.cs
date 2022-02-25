using System.Net;
using AuctionMarket.Server.Application.Abstractions;
using AuctionMarket.Server.Domain.Commands;
using AuctionMarket.Server.Domain.Entities;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuctionMarket.Server.Application.Commands;

public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly HttpContext _httpContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public RegisterAccountCommandHandler(IAppDbContext dbContext, IHttpContextAccessor httpContextAccessor,
        IPasswordHasher<User> passwordHasher)
        => (_dbContext, _httpContext, _passwordHasher) = (dbContext, httpContextAccessor.HttpContext!, passwordHasher);

    public async Task<Unit> Handle(RegisterAccountCommand command, CancellationToken cancellationToken)
    {
        if (_httpContext.User.Identity?.IsAuthenticated == true)
            throw new ProblemDetailsException((int)HttpStatusCode.BadRequest, "You are already logged in.");

        if (await _dbContext.Users.AnyAsync(u => u.UserName == command.UserName, cancellationToken))
            throw new ProblemDetailsException((int)HttpStatusCode.BadRequest, "Username already taken.");

        var user = new User
        {
            UserName = command.UserName,
            FirstName = command.FirstName,
            LastName = command.LastName
        };

        var userEntry = await _dbContext.Users.AddAsync(user, cancellationToken);
        userEntry.Entity.PasswordHash = _passwordHasher.HashPassword(userEntry.Entity, command.Password);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}