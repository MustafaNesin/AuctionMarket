namespace AuctionMarket.Shared.Domain.Abstractions.Commands;

public abstract class RegisterAccountCommandBase
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}