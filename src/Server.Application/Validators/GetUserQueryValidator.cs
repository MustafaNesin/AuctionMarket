using AuctionMarket.Server.Domain.Queries;
using AuctionMarket.Shared.Application.Validators;
using FluentValidation;

namespace AuctionMarket.Server.Application.Validators;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator() => Include(new GetUserQueryValidatorBase());
}