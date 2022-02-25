using AuctionMarket.Server.Domain.Commands;
using AuctionMarket.Shared.Application.Validators;
using FluentValidation;

namespace AuctionMarket.Server.Application.Validators;

public class BidCommandValidator : AbstractValidator<BidCommand>
{
    public BidCommandValidator() => Include(new BidCommandValidatorBase());
}