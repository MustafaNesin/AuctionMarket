using AuctionMarket.Shared.Domain.Abstractions.Commands;
using FluentValidation;

namespace AuctionMarket.Shared.Application.Validators;

public class LogoutAccountCommandValidatorBase : AbstractValidator<LogoutAccountCommandBase>
{
}