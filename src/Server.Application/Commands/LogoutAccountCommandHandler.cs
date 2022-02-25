using AuctionMarket.Server.Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace AuctionMarket.Server.Application.Commands;

public class LogoutAccountCommandHandler : IRequestHandler<LogoutAccountCommand>
{
    private readonly HttpContext _httpContext;

    public LogoutAccountCommandHandler(IHttpContextAccessor httpContextAccessor)
        => _httpContext = httpContextAccessor.HttpContext!;

    public async Task<Unit> Handle(LogoutAccountCommand command, CancellationToken cancellationToken)
    {
        await _httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Unit.Value;
    }
}