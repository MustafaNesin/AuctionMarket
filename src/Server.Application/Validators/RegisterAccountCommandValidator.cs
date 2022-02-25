using AuctionMarket.Server.Domain.Commands;
using AuctionMarket.Shared.Application.Validators;
using FluentValidation;

namespace AuctionMarket.Server.Application.Validators;

public class RegisterAccountCommandValidator : AbstractValidator<RegisterAccountCommand>
{
    public RegisterAccountCommandValidator() => Include(new RegisterAccountCommandValidatorBase());
}