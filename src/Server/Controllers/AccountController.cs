using AuctionMarket.Server.Abstractions;
using AuctionMarket.Server.Domain.Commands;
using AuctionMarket.Server.Domain.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionMarket.Server.Controllers;

public class AccountController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync() => Ok(await Mediator.Send(GetAccountQuery.Unit));

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> LoginAsync(LoginAccountCommand command) => Ok(await Mediator.Send(command));

    [HttpPost]
    public async Task<IActionResult> LogoutAsync() => Ok(await Mediator.Send(LogoutAccountCommand.Unit));

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegisterAccountCommand command) => Ok(await Mediator.Send(command));
}