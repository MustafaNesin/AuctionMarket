using AuctionMarket.Shared.Domain.Abstractions.Commands;
using FluentValidation;

namespace AuctionMarket.Shared.Application.Validators;

public class EditAuctionCommandValidatorBase : AbstractValidator<EditAuctionCommandBase>
{
    public EditAuctionCommandValidatorBase()
    {
        RuleFor(command => command.Auction).NotNull();
        RuleFor(command => command.Auction.Title).NotEmpty();
    }
}