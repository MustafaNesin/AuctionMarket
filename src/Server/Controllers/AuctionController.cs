using AuctionMarket.Server.Abstractions;
using AuctionMarket.Server.Domain.Commands;
using AuctionMarket.Server.Domain.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AuctionMarket.Server.Controllers;

public class AuctionController : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateAuctionCommand command) => Ok(await Mediator.Send(command));
    
    [HttpPost]
    public async Task<IActionResult> EditAsync(EditAuctionCommand command) => Ok(await Mediator.Send(command));

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] GetAuctionQuery query) => Ok(await Mediator.Send(query));

    [HttpGet]
    public async Task<IActionResult> GetListAsync([FromQuery] GetAuctionListQuery query)
        => Ok(await Mediator.Send(query));
}