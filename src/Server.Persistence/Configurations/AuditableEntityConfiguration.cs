using AuctionMarket.Server.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionMarket.Server.Persistence.Configurations;

public abstract class AuditableEntityConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
    where TEntity : AuditableEntity<TId>
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.UpdatedAt).IsConcurrencyToken();

        // TODO: Eğer override olmuyor ise CreatedBy config sil
        builder.HasOne(entity => entity.CreatedBy)
            .WithMany()
            .HasForeignKey(entity => entity.CreatedById);

        builder.HasOne(entity => entity.DeletedBy)
            .WithMany()
            .HasForeignKey(entity => entity.DeletedById);

        builder.HasOne(entity => entity.UpdatedBy)
            .WithMany()
            .HasForeignKey(entity => entity.UpdatedById);
    }
}