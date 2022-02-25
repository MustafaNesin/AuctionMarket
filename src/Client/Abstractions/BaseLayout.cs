using AuctionMarket.Client.Application.Abstractions;
using Microsoft.AspNetCore.Components;

namespace AuctionMarket.Client.Abstractions;

public class BaseLayout : LayoutComponentBase, IAsyncDisposable
{
    [Inject]
    protected IStateContainerService StateContainer { get; private set; } = default!;

    public virtual ValueTask DisposeAsync()
    {
        StateContainer.StateHasChanged -= StateHasChanged;
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }

    protected override void OnInitialized() => StateContainer.StateHasChanged += StateHasChanged;
}