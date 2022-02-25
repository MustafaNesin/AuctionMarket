using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Client.Shared;
using AuctionMarket.Shared.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AuctionMarket.Client.Abstractions;

public class BaseComponent : ComponentBase, IAsyncDisposable
{
    [Inject]
    protected IDialogService DialogService { get; private set; } = default!;

    [Inject]
    protected IMediator Mediator { get; private set; } = default!;

    [Inject]
    protected ISnackbar Snackbar { get; private set; } = default!;

    [Inject]
    protected IStateContainerService StateContainer { get; private set; } = default!;

    public virtual ValueTask DisposeAsync()
    {
        StateContainer.StateHasChanged -= StateHasChanged;
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }

    protected override void OnInitialized() => StateContainer.StateHasChanged += StateHasChanged;

    protected void ShowProblem(ProblemDetails problemDetails, bool showDialog)
    {
        if (showDialog)
            ShowProblemDialog(problemDetails);

        Snackbar.Add(problemDetails.Title ?? "Error", Severity.Error, options =>
            options.Onclick = _ =>
            {
                ShowProblemDialog(problemDetails);
                return Task.CompletedTask;
            });
    }

    private void ShowProblemDialog(ProblemDetails problemDetails)
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        var parameters = new DialogParameters { [nameof(ProblemDialogComponent.ProblemDetails)] = problemDetails };

        DialogService.Show<ProblemDialogComponent>("Error", parameters, options);
    }
}