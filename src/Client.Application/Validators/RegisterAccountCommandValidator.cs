using AuctionMarket.Client.Domain.Commands;
using AuctionMarket.Shared.Application.Validators;

namespace AuctionMarket.Client.Application.Validators;

public class RegisterAccountCommandValidator : ClientSideValidatorBase<RegisterAccountCommand>
{
    public RegisterAccountCommandValidator() => Include(new RegisterAccountCommandValidatorBase());
}