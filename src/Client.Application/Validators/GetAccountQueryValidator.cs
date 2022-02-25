using AuctionMarket.Client.Domain.Queries;
using AuctionMarket.Shared.Application.Validators;

namespace AuctionMarket.Client.Application.Validators;

public class GetAccountQueryValidator : ClientSideValidatorBase<GetAccountQuery>
{
    public GetAccountQueryValidator() => Include(new GetAccountQueryValidatorBase());
}