using AuctionMarket.Client.Domain.Commands;
using AuctionMarket.Shared.Application.Validators;

namespace AuctionMarket.Client.Application.Validators;

public class BidCommandValidator : ClientSideValidatorBase<BidCommand>
{
    public BidCommandValidator() => Include(new BidCommandValidatorBase());
}