using AuctionMarket.Client.Domain.Commands;
using AuctionMarket.Shared.Application.Validators;
using FluentValidation;

namespace AuctionMarket.Client.Application.Validators;

public class EditAuctionCommandValidator : ClientSideValidatorBase<EditAuctionCommand>
{
    public EditAuctionCommandValidator() => Include(new EditAuctionCommandValidatorBase());
}