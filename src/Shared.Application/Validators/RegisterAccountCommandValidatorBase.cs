using AuctionMarket.Shared.Domain.Abstractions.Commands;
using FluentValidation;

namespace AuctionMarket.Shared.Application.Validators;

public class RegisterAccountCommandValidatorBase : AbstractValidator<RegisterAccountCommandBase>
{
    public RegisterAccountCommandValidatorBase()
    {
        RuleFor(command => command.FirstName).NotEmpty();
        RuleFor(command => command.LastName).NotEmpty();
        RuleFor(command => command.UserName).NotEmpty();
        RuleFor(command => command.Password).NotEmpty().MinimumLength(6);
    }
}