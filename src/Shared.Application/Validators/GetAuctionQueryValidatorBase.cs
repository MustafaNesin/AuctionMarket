using AuctionMarket.Shared.Domain.Abstractions.Queries;
using FluentValidation;

namespace AuctionMarket.Shared.Application.Validators;

public class GetAuctionQueryValidatorBase : AbstractValidator<GetAuctionQueryBase>
{
    public GetAuctionQueryValidatorBase() => RuleFor(query => query.Id).GreaterThan(0);
}