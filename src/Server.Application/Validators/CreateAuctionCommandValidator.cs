using AuctionMarket.Server.Domain.Commands;
using AuctionMarket.Shared.Application.Validators;
using FluentValidation;

namespace AuctionMarket.Server.Application.Validators;

public class CreateAuctionCommandValidator : AbstractValidator<CreateAuctionCommand>
{
    public CreateAuctionCommandValidator()
    {
        Include(new CreateAuctionCommandValidatorBase());
        RuleFor(command => command.Auction.StartsAt).NotNull().GreaterThan(DateTime.UtcNow);
        RuleFor(command => command.Auction.EndsAt).NotNull().GreaterThan(command => command.Auction.StartsAt);
    }
}