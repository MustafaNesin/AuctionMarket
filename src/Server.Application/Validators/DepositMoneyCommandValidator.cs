using AuctionMarket.Server.Domain.Commands;
using AuctionMarket.Shared.Application.Validators;
using FluentValidation;

namespace AuctionMarket.Server.Application.Validators;

public class DepositMoneyCommandValidator : AbstractValidator<DepositMoneyCommand>
{
    public DepositMoneyCommandValidator() => Include(new DepositMoneyCommandValidatorBase());
}