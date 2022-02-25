using AuctionMarket.Client.Domain.Queries;
using AuctionMarket.Shared.Application.Validators;

namespace AuctionMarket.Client.Application.Validators;

public class GetAuctionQueryValidator : ClientSideValidatorBase<GetAuctionQuery>
{
    public GetAuctionQueryValidator() => Include(new GetAuctionQueryValidatorBase());
}