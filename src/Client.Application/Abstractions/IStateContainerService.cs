using AuctionMarket.Shared.Domain.DTOs;

namespace AuctionMarket.Client.Application.Abstractions;

public interface IStateContainerService
{
    IHostAuthenticationStateProvider Authentication { get; }
    UserDto User { get; set; }

    event Action? StateHasChanged;
}