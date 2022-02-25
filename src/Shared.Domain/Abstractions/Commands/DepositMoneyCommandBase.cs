namespace AuctionMarket.Shared.Domain.Abstractions.Commands;

public abstract class DepositMoneyCommandBase
{
    public double Value { get; set; }
}