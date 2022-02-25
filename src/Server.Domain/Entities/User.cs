using AuctionMarket.Server.Domain.Abstractions;
using AuctionMarket.Shared.Domain.ValueObjects;

namespace AuctionMarket.Server.Domain.Entities;

public sealed class User : AuditableEntity<Guid>
{
    public User()
    {
        Id = Guid.NewGuid();
        SecurityStamp = Guid.NewGuid();
    }

    public double Balance { get; set; }
    public string? Biography { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public Role? Role { get; set; }
    public Guid SecurityStamp { get; set; }
    public string UserName { get; set; } = default!;

    public ICollection<Auction> Auctions { get; set; } = default!;
    public ICollection<Bid> Bids { get; set; } = default!;
}