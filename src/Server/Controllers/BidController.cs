using AuctionMarket.Server.Abstractions;
using AuctionMarket.Server.Domain.Commands;
using Microsoft.AspNetCore.Mvc;

namespace AuctionMarket.Server.Controllers;

public class BidController : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(BidCommand command) => Ok(await Mediator.Send(command));
}