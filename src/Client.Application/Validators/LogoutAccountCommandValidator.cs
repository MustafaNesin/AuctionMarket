using AuctionMarket.Client.Domain.Commands;
using AuctionMarket.Shared.Application.Validators;

namespace AuctionMarket.Client.Application.Validators;

public class LogoutAccountCommandValidator : ClientSideValidatorBase<LogoutAccountCommand>
{
    public LogoutAccountCommandValidator() => Include(new LogoutAccountCommandValidatorBase());
}