using AuctionMarket.Shared.Domain.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AuctionMarket.Client.Shared;

public partial class ProblemDialogComponent
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public ProblemDetails? ProblemDetails { get; set; }

    private void Close() => MudDialog.Close();
}