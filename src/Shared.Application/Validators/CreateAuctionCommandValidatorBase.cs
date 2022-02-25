using AuctionMarket.Shared.Domain.Abstractions.Commands;
using FluentValidation;

namespace AuctionMarket.Shared.Application.Validators;

public class CreateAuctionCommandValidatorBase : AbstractValidator<CreateAuctionCommandBase>
{
    public CreateAuctionCommandValidatorBase()
    {
        RuleFor(command => command.Auction).NotNull();
        RuleFor(command => command.Auction.Title).NotEmpty();
        RuleFor(command => command.Auction.StartingPrice).GreaterThan(0);
        RuleFor(command => command.Auction.MinBidIncrement).GreaterThan(0);
    }
}