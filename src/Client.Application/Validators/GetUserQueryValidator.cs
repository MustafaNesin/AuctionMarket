using AuctionMarket.Client.Domain.Queries;
using AuctionMarket.Shared.Application.Validators;

namespace AuctionMarket.Client.Application.Validators;

public class GetUserQueryValidator : ClientSideValidatorBase<GetUserQuery>
{
    public GetUserQueryValidator() => Include(new GetUserQueryValidatorBase());
}