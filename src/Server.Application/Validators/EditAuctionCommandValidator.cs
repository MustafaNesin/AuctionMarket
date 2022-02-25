using AuctionMarket.Server.Domain.Commands;
using AuctionMarket.Shared.Application.Validators;
using FluentValidation;

namespace AuctionMarket.Server.Application.Validators;

public class EditAuctionCommandValidator : AbstractValidator<EditAuctionCommand>
{
    public EditAuctionCommandValidator() => Include(new EditAuctionCommandValidatorBase());
}