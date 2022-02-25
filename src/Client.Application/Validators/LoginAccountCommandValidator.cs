using AuctionMarket.Client.Domain.Commands;
using AuctionMarket.Shared.Application.Validators;

namespace AuctionMarket.Client.Application.Validators;

public class LoginAccountCommandValidator : ClientSideValidatorBase<LoginAccountCommand>
{
    public LoginAccountCommandValidator() => Include(new LoginAccountCommandValidatorBase());
}