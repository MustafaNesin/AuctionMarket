using System.Net;
using System.Security.Claims;
using AuctionMarket.Server.Application.Abstractions;
using AuctionMarket.Server.Domain.Commands;
using AuctionMarket.Server.Domain.Entities;
using AuctionMarket.Shared.Domain.DTOs;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuctionMarket.Server.Application.Commands;

public class LoginAccountCommandHandler : IRequestHandler<LoginAccountCommand, AccountDto>
{
    private readonly IAppDbContext _dbContext;
    private readonly HttpContext _httpContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public LoginAccountCommandHandler(IAppDbContext dbContext, IHttpContextAccessor httpContextAccessor,
        IPasswordHasher<User> passwordHasher)
        => (_dbContext, _httpContext, _passwordHasher) = (dbContext, httpContextAccessor.HttpContext!, passwordHasher);

    public async Task<AccountDto> Handle(LoginAccountCommand command, CancellationToken cancellationToken)
    {
        if (_httpContext.User.Identity?.IsAuthenticated == true)
            return new AccountDto(_httpContext.User.Claims.ToDictionary(c => c.Type, c => c.Value));

        var user = await _dbContext.Users.SingleOrDefaultAsync(
            u => u.UserName == command.UserName, cancellationToken);

        if (user is null || user.IsDeleted)
            throw new ProblemDetailsException((int)HttpStatusCode.BadRequest, "User not found.");

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, command.Password);
        if (verificationResult == PasswordVerificationResult.Failed)
            throw new ProblemDetailsException((int)HttpStatusCode.BadRequest, "Incorrect password.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName),
            // TODO: SecurityStamp client'a gönderilmeyebilir
            new(nameof(User.SecurityStamp), user.SecurityStamp.ToString())
        };

        if (user.Role is not null)
            claims.Add(new Claim(ClaimTypes.Role, user.Role.Name));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await _httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = command.RememberLogin });

        return new AccountDto(claims.ToDictionary(c => c.Type, c => c.Value));
    }
}