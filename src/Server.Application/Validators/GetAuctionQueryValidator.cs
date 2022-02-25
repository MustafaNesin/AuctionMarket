using AuctionMarket.Server.Domain.Queries;
using AuctionMarket.Shared.Application.Validators;
using FluentValidation;

namespace AuctionMarket.Server.Application.Validators;

public class GetAuctionQueryValidator : AbstractValidator<GetAuctionQuery>
{
    public GetAuctionQueryValidator() => Include(new GetAuctionQueryValidatorBase());
}