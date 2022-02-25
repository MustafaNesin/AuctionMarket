using AuctionMarket.Client.Domain.Commands;
using AuctionMarket.Shared.Application.Validators;

namespace AuctionMarket.Client.Application.Validators;

public class DepositMoneyCommandValidator : ClientSideValidatorBase<DepositMoneyCommand>
{
    public DepositMoneyCommandValidator() => Include(new DepositMoneyCommandValidatorBase());
}