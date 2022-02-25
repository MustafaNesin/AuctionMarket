using AuctionMarket.Server.Domain.Commands;
using AuctionMarket.Shared.Application.Validators;
using FluentValidation;

namespace AuctionMarket.Server.Application.Validators;

public class LoginAccountCommandValidator : AbstractValidator<LoginAccountCommand>
{
    public LoginAccountCommandValidator() => Include(new LoginAccountCommandValidatorBase());
}