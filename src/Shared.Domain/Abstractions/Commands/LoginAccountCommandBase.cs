namespace AuctionMarket.Shared.Domain.Abstractions.Commands;

public abstract class LoginAccountCommandBase
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public bool RememberLogin { get; set; }
}