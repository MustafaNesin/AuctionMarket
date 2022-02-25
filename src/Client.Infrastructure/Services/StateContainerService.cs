using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Shared.Domain.DTOs;

namespace AuctionMarket.Client.Infrastructure.Services;

public class StateContainerService : IStateContainerService
{
    private UserDto _user = default!;

    public StateContainerService(IHostAuthenticationStateProvider authenticationStateProvider)
        => Authentication = authenticationStateProvider;

    public IHostAuthenticationStateProvider Authentication { get; }

    public UserDto User
    {
        get => _user;
        set
        {
            _user = value;
            NotifyStateHasChanged();
        }
    }

    public event Action? StateHasChanged;

    private void NotifyStateHasChanged() => StateHasChanged?.Invoke();
}