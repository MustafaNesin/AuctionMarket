using AuctionMarket.Server.Domain.Entities;

namespace AuctionMarket.Server.Domain.Abstractions;

public abstract class AuditableEntity : IEntity
{
    public DateTime? CreatedAt { get; set; }
    public User? CreatedBy { get; set; }
    public Guid? CreatedById { get; set; }
    public DateTime? DeletedAt { get; set; }
    public User? DeletedBy { get; set; }
    public Guid? DeletedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public User? UpdatedBy { get; set; }
    public Guid? UpdatedById { get; set; }

    public bool IsDeleted => DeletedAt is not null;
}