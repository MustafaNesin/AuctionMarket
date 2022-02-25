using AuctionMarket.Shared.Domain.Abstractions.Commands;
using FluentValidation;

namespace AuctionMarket.Shared.Application.Validators;

public class DepositMoneyCommandValidatorBase : AbstractValidator<DepositMoneyCommandBase>
{
    public DepositMoneyCommandValidatorBase() => RuleFor(command => command.Value).GreaterThanOrEqualTo(10.0);
}