using AuctionMarket.Client.Domain.Commands;
using AuctionMarket.Shared.Application.Validators;
using FluentValidation;

namespace AuctionMarket.Client.Application.Validators;

public class CreateAuctionCommandValidator : ClientSideValidatorBase<CreateAuctionCommand>
{
    public CreateAuctionCommandValidator()
    {
        Include(new CreateAuctionCommandValidatorBase());
        RuleFor(command => command.Auction.StartsAt).NotNull().GreaterThan(DateTime.Now);
        RuleFor(command => command.Auction.EndsAt).NotNull().GreaterThan(command => command.Auction.StartsAt);
    }
}