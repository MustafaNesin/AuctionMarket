using AuctionMarket.Shared.Domain.Abstractions.Commands;
using FluentValidation;

namespace AuctionMarket.Shared.Application.Validators;

public class BidCommandValidatorBase : AbstractValidator<BidCommandBase>
{
    public BidCommandValidatorBase()
    {
        RuleFor(command => command.AuctionId).GreaterThan(0);
        RuleFor(command => command.Value).GreaterThan(0);
    }
}