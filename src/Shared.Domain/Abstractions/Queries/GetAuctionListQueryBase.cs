using AuctionMarket.Shared.Domain.Enumerations;

namespace AuctionMarket.Shared.Domain.Abstractions.Queries;

public abstract record GetAuctionListQueryBase(
    AuctionStatus? Status, string? CreatorName, string? BidderName, string? WinnerName, string? SearchQuery)
{
    public const string TitleSortLabel = "s_title";
    public const string CreatorSortLabel = "s_creator";
    public const string CreatedAtSortLabel = "s_createdat";
    public const string StartingPriceSortLabel = "s_starting";
    public const string MinIncrementSortLabel = "s_increment";
    public const string LastBidderSortLabel = "s_bidder";
    public const string LastBidAtSortLabel = "s_bidat";
    public const string LastBidValueSortLabel = "s_bid";
    public const string StartsAtSortLabel = "s_startsat";
    public const string EndsAtSortLabel = "s_endsat";
    public const string StatusSortLabel = "s_status";

    public static bool IsValidSortLabel(string? sortLabel)
        => sortLabel is null
            or TitleSortLabel
            or CreatorSortLabel
            or CreatedAtSortLabel
            or StartingPriceSortLabel
            or MinIncrementSortLabel
            or LastBidderSortLabel
            or LastBidAtSortLabel
            or LastBidValueSortLabel
            or StartsAtSortLabel
            or EndsAtSortLabel
            or StatusSortLabel;
}