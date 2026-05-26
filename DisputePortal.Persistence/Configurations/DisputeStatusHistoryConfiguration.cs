using DisputePortal.Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class DisputeStatusHistoryConfiguration : IEntityTypeConfiguration<DisputeStatusHistory>
{
    public void Configure(EntityTypeBuilder<DisputeStatusHistory> builder)
    {
        builder.ToTable("dispute_status_histories", "data");
        builder.HasKey(h => h.Id);
        builder.Property(h => h.FromStatus).HasConversion<string>().HasMaxLength(50);
        builder.Property(h => h.ToStatus).HasConversion<string>().HasMaxLength(50);
        builder.Property(h => h.ChangedByUserId).HasMaxLength(100).IsRequired();
        builder.Property(h => h.ChangedByRole).HasMaxLength(50).IsRequired();
        builder.Property(h => h.Notes).HasMaxLength(2000);
        builder.HasIndex(h => new { h.DisputeId, h.ChangedAt });
    }
}
