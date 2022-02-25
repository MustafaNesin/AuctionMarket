using AuctionMarket.Server.Domain.Entities;
using AuctionMarket.Shared.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionMarket.Server.Persistence.Configurations;

public class UserConfiguration : AuditableEntityConfiguration<User, Guid>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.PasswordHash)
            .HasColumnType("TEXT COLLATE NOCASE");

        builder.Property(p => p.Role)
            .HasConversion(r => r == null ? null : (string?)r.Name, n => n == null ? null : new Role(n))
            .HasColumnType("TEXT COLLATE NOCASE");

        builder.Property(p => p.SecurityStamp)
            .HasColumnType("TEXT COLLATE NOCASE");

        builder.Property(p => p.UserName)
            .HasColumnType("TEXT COLLATE NOCASE");
    }
}