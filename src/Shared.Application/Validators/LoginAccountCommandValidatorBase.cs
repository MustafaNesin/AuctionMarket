using AuctionMarket.Shared.Domain.Abstractions.Commands;
using FluentValidation;

namespace AuctionMarket.Shared.Application.Validators;

public class LoginAccountCommandValidatorBase : AbstractValidator<LoginAccountCommandBase>
{
    public LoginAccountCommandValidatorBase()
    {
        RuleFor(command => command.UserName).NotEmpty();
        RuleFor(command => command.Password).NotEmpty();
    }
}