using DisputePortal.Application.Domain.Enums;
using DisputePortal.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class DisputeReasonConfiguration : IEntityTypeConfiguration<DisputeReasonCodeModel>
{
    public void Configure(EntityTypeBuilder<DisputeReasonCodeModel> builder)
    {
        builder.ToTableName(DbConstants.StaticSchema);

        builder.Property(r => r.Name).HasPostgresVarChar(256).IsRequired();
        builder.Property(r => r.Description).HasPostgresVarChar(512).IsRequired();

        builder.HasIndex(r => r.Name).IsUnique();

        builder.HasData(Enum.GetValues<DisputeReasonCode>()
            .Select(r => r.ToModel())
            .ToArray());
    }
}
