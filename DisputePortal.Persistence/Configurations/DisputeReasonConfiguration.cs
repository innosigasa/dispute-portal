using DisputePortal.Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class DisputeReasonConfiguration : IEntityTypeConfiguration<DisputeReason>
{
    public void Configure(EntityTypeBuilder<DisputeReason> builder)
    {
        builder.ToTable("dispute_reasons", "static");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Code).HasMaxLength(100).IsRequired();
        builder.Property(r => r.Label).HasMaxLength(200).IsRequired();
        builder.HasIndex(r => r.Code).IsUnique();
    }
}
