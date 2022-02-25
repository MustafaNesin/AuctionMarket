namespace AuctionMarket.Shared.Domain.ValueObjects;

public record Role(string Name)
{
    public static readonly Role Administrator = new("Admin");
}