using AuctionMarket.Server.Domain.Commands;
using AuctionMarket.Shared.Application.Validators;
using FluentValidation;

namespace AuctionMarket.Server.Application.Validators;

public class LogoutAccountCommandValidator : AbstractValidator<LogoutAccountCommand>
{
    public LogoutAccountCommandValidator() => Include(new LogoutAccountCommandValidatorBase());
}