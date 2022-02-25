using AuctionMarket.Client.Application.Validators;
using AuctionMarket.Client.Domain.Commands;
using AuctionMarket.Client.Domain.Queries;
using AuctionMarket.Shared.Domain.DTOs;
using AuctionMarket.Shared.Domain.Enumerations;
using AuctionMarket.Shared.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace AuctionMarket.Client.Pages;

public partial class EditAuctionPage
{
    private readonly EditAuctionCommand _command = new();
    private readonly EditAuctionCommandValidator _validator = new();
    private MudForm _form = default!;
    private bool _isSubmitDisabled;

    [Parameter]
    public int Id { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;


    protected override async Task OnParametersSetAsync()
    {
        var (isSuccess, auction, problemDetails) = await Mediator.Send(new GetAuctionQuery(Id));

        if (isSuccess)
        {
            _command.Auction = auction;
            _command.Auction.CreatedAt = _command.Auction.CreatedAt!.Value.ToLocalTime();
            _command.Auction.StartsAt = _command.Auction.StartsAt!.Value.ToLocalTime();
            _command.Auction.EndsAt = _command.Auction.EndsAt!.Value.ToLocalTime();
        }
        else
        {
            ShowProblem(problemDetails, false);
            NavigationManager.NavigateTo("/Auction/List");
        }
    }
    
    private async Task OnSubmitAsync()
    {
        _isSubmitDisabled = true;
        await _form.Validate();

        if (!_form.IsValid)
        {
            _isSubmitDisabled = false;
            return;
        }

        var command = new EditAuctionCommand
        {
            Auction = new AuctionDto
            {
                Id = _command.Auction.Id,
                Title = _command.Auction.Title,
                Description = _command.Auction.Description
            }
        };

        if (_command.Auction.GetStatus(DateTime.Now) == AuctionStatus.NotStarted)
        {
            command.Auction.StartingPrice = _command.Auction.StartingPrice;
            command.Auction.MinBidIncrement = _command.Auction.MinBidIncrement;
            command.Auction.StartsAt = _command.Auction.StartsAt!.Value.ToUniversalTime();
            command.Auction.EndsAt = _command.Auction.EndsAt!.Value.ToUniversalTime();
        }
        
        var (isSuccess, problemDetails) = await Mediator.Send(command);

        if (isSuccess)
        {
            NavigationManager.NavigateTo("/Auction/Detail/" + command.Auction.Id);
            Snackbar.Add("Auction edited successfully!", Severity.Success);
        }
        else
            ShowProblem(problemDetails, true);

        _isSubmitDisabled = false;
    }
}