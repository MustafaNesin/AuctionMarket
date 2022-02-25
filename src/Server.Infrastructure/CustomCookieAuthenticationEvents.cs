using System.Security.Claims;
using AuctionMarket.Server.Application.Abstractions;
using AuctionMarket.Server.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AuctionMarket.Server.Infrastructure;

public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
{
    private readonly IAppDbContext _dbContext;

    public CustomCookieAuthenticationEvents(IAppDbContext dbContext) => _dbContext = dbContext;

    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        if (context.Principal is null)
        {
            await RejectAndSignOutAsync();
            return;
        }

        var idStr = context.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var securityStampStr = context.Principal.FindFirstValue(nameof(User.SecurityStamp));

        if (idStr is null || securityStampStr is null)
        {
            await RejectAndSignOutAsync();
            return;
        }

        var id = Guid.Parse(idStr);
        var securityStamp = Guid.Parse(securityStampStr);

        // TODO: Buraya bir elapsed time kontrolü gelmeli

        var user = await _dbContext.Users.SingleOrDefaultAsync(
            u => u.Id == id && u.SecurityStamp == securityStamp);

        if (user is null || user.IsDeleted)
            await RejectAndSignOutAsync();

        async Task RejectAndSignOutAsync()
        {
            context.RejectPrincipal();
            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }

    public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    }

    public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    }
}