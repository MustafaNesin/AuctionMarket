using AuctionMarket.Client.Domain.Queries;
using AuctionMarket.Shared.Application.Validators;
using AuctionMarket.Shared.Domain.Abstractions.Queries;
using FluentValidation;

namespace AuctionMarket.Client.Application.Validators;

public class GetAuctionListQueryValidator : ClientSideValidatorBase<GetAuctionListQuery>
{
    public GetAuctionListQueryValidator()
    {
        Include(new GetAuctionListQueryValidatorBase());
        RuleFor(query => query.TableState).NotNull();
        RuleFor(query => query.TableState.Page).GreaterThanOrEqualTo(0);
        RuleFor(query => query.TableState.PageSize).GreaterThan(0);
        RuleFor(query => query.TableState.SortDirection).IsInEnum();
        RuleFor(query => query.TableState.SortLabel).Must(GetAuctionListQueryBase.IsValidSortLabel);
    }
}