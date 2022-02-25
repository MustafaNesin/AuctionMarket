using AuctionMarket.Client.Application.Validators;
using AuctionMarket.Client.Domain.Commands;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AuctionMarket.Client.Pages;

public partial class CreateAuctionPage
{
    private readonly CreateAuctionCommand _command = new();
    private readonly CreateAuctionCommandValidator _validator = new();
    private MudForm _form = default!;
    private bool _isSubmitDisabled;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private async Task OnSubmitAsync()
    {
        _isSubmitDisabled = true;
        await _form.Validate();

        if (!_form.IsValid)
        {
            _isSubmitDisabled = false;
            return;
        }
        
        _command.Auction.StartsAt = _command.Auction.StartsAt!.Value.ToUniversalTime();
        _command.Auction.EndsAt = _command.Auction.EndsAt!.Value.ToUniversalTime();

        var (isSuccess, auctionId, problemDetails) = await Mediator.Send(_command);

        if (isSuccess)
        {
            NavigationManager.NavigateTo("/Auction/Detail/" + auctionId);
            Snackbar.Add("Auction submitted successfully!", Severity.Success);
        }
        else
        {
            _command.Auction.StartsAt = _command.Auction.StartsAt!.Value.ToLocalTime();
            _command.Auction.EndsAt = _command.Auction.EndsAt!.Value.ToLocalTime();
            ShowProblem(problemDetails, true);
        }

        _isSubmitDisabled = true;
    }
}