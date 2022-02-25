namespace AuctionMarket.Server.Application.Abstractions;

public interface ICurrentAccountService
{
    Guid? GetId();
    Guid? GetSecurityStamp();
}