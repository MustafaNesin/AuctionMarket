using AuctionMarket.Server.Abstractions;
using AuctionMarket.Server.Domain.Commands;
using AuctionMarket.Server.Domain.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AuctionMarket.Server.Controllers;

public class UserController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] GetUserQuery query) => Ok(await Mediator.Send(query));

    [HttpPost]
    public async Task<IActionResult> DepositAsync(DepositMoneyCommand command) => Ok(await Mediator.Send(command));
}