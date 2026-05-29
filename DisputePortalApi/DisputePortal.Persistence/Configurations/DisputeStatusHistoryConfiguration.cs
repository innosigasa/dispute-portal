using DisputePortal.Application.Domain.Models;
using DisputePortal.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class DisputeStatusHistoryConfiguration : IEntityTypeConfiguration<DisputeStatusHistoryModel>
{
    public void Configure(EntityTypeBuilder<DisputeStatusHistoryModel> builder)
    {
        builder.ToTableName(DbConstants.DataSchema);

        builder.Property(h => h.ChangedByUserId).HasPostgresVarChar(256).IsRequired();
        builder.Property(h => h.ChangedByRole).HasPostgresVarChar(256).IsRequired();
        builder.Property(h => h.Notes).HasPostgresText();

        builder.HasIndex(h => new { h.DisputeId, h.ChangedAt });
    }
}
