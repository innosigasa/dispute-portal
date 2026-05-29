using DisputePortal.Application.Domain.Models;
using DisputePortal.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class DisputeConfiguration : IEntityTypeConfiguration<DisputeModel>
{
    public void Configure(EntityTypeBuilder<DisputeModel> builder)
    {
        builder.ToTableName(DbConstants.DataSchema);

        builder.Property(d => d.ReferenceNumber).HasPostgresVarChar(256).IsRequired();
        builder.Property(d => d.Comments).HasPostgresText();

        builder.HasIndex(d => d.ReferenceNumber).IsUnique();
        builder.HasIndex(d => new { d.CustomerId, Status = d.StatusId });
        
        builder.HasOne(d => d.Customer)
            .WithMany(c => c.Disputes)
            .HasForeignKey(d => d.CustomerId);
        
        builder.HasMany(d => d.StatusHistory)
            .WithOne(h => h.Dispute)
            .HasForeignKey(h => h.DisputeId);

        builder.HasMany(d => d.Notifications)
            .WithOne(n => n.Dispute)
            .HasForeignKey(n => n.DisputeId);
    }
}
